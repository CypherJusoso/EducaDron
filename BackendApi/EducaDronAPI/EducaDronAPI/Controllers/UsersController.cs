using EducaDronAPI.Data;
using EducaDronAPI.DTOs;
using EducaDronAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EducaDronAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;


        public UsersController(AppDbContext context, SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public List<Users> GetUsers()
        {
            return _context.Users.OrderByDescending(c => c.Id).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    UserName = model.Name,
                    Email = model.Email,
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if (result.Succeeded) 
                {
                    var initialProgress = new List<Progress>
            {
                new Progress { UsuarioId = users.Id, Nivel = 1, Estado = "desbloqueado" },
                new Progress { UsuarioId = users.Id, Nivel = 2, Estado = "bloqueado" },
                new Progress { UsuarioId = users.Id, Nivel = 3, Estado = "bloqueado" },
            };

                    foreach (var newProgress in initialProgress)
                    {
                        _context.Progress.Add(newProgress);
                    }
                    await _context.SaveChangesAsync();

                    var userDto = new UserDto
                    {
                        Id = users.Id,
                        UserName = users.UserName,
                        Email = users.Email,
                    };
                    return Ok(userDto);
                }
                else
                {

                    var errorList = result.Errors.Select(e => e.Description).ToArray();

                    return BadRequest(new { errors = errorList});
                }
            }
            return BadRequest(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                    });
                }
                else
                {
                    return BadRequest( new
                    {
                        errors = new[] { "El email o la contraseña son incorrectos." }
                    });
                }
            }
            return BadRequest(model);
        }

        [HttpPost("logout")] 
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new { mensaje = "Has cerrado sesion." });
        }
    }
}
