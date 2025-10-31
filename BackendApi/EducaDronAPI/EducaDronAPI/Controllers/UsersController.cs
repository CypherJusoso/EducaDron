using EducaDronAPI.Data;
using EducaDronAPI.DTOs;
using EducaDronAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Devuelve a todos los usuarios registrados en una lista
        /// </summary>
        [HttpGet]
        public List<Users> GetUsers()
        {
            return _context.Users.OrderByDescending(c => c.Id).ToList();
        }
        /// <summary>
        /// Devuelve un usuario por ID
        /// </summary>
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
        /// <summary>
        /// Registra un usuario en la base de datos.
        /// Crea registros iniciales de progress y level point para
        /// cada nivel
        /// </summary>
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

                    var initalPoints = new List<LevelPoint>
                    {
                        new LevelPoint { UsuarioId = users.Id, Level = 1, Points = 0},
                        new LevelPoint { UsuarioId = users.Id, Level = 2, Points = 0},
                        new LevelPoint { UsuarioId = users.Id, Level = 3, Points = 0},
                    };
                    foreach (var newPoints in initalPoints)
                    {
                        _context.LevelPoint.Add(newPoints);
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
        /// <summary>
        /// Inicia sesion verificando el nombre de usuario y la contraseña
        /// </summary>
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
                        errors = new[] { "El nombre de usuario o la contraseña son incorrectos." }
                    });
                }
            }
            return BadRequest(model);
        }
        /// <summary>
        /// Cierra la sesion del usuario
        /// </summary>
        [HttpPost("logout")] 
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new { mensaje = "Has cerrado sesion." });
        }
        /// <summary>
        /// Actualiza los puntos del usuario en un nivel especifico.
        /// Controla que solo se actualicen los puntos si superan a
        /// los anteriores
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("points/update-points")]
        public async Task<IActionResult> UpdateLevelPoint(LevelPointDto dto)
        {
            if (dto == null) { return BadRequest(); }

            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null) { return NotFound("Usuario no encontrado."); }

            var levelToUpdatePoints = await _context.LevelPoint.Where(lp => lp.UsuarioId == dto.UserId && lp.Level == dto.LevelId).FirstOrDefaultAsync();
            
            if(levelToUpdatePoints == null) { return NotFound("Nivel no encontrado."); }
            
            if ( dto.NewPoints > levelToUpdatePoints.Points)
            {
                levelToUpdatePoints.Points = dto.NewPoints;
            }
            
            await _context.SaveChangesAsync();

            return Ok(new
            {
                levelToUpdatePoints.Level,
                levelToUpdatePoints.Points
            });
        }
        /// <summary>
        /// Obtiene los puntos del usuario, separados por nivel
        /// y el total
        /// </summary>
        [HttpGet("points/{userId}")]
        public async Task<IActionResult> GetUserPoints(string userId)
        {
            if (string.IsNullOrEmpty(userId)) {  return BadRequest("El UserId no puede estar vacio."); }

            var points = await _context.LevelPoint
                .Where(lp => lp.UsuarioId.Equals(userId))
                .Select(lp => new { lp.Level, lp.Points})
                .ToListAsync();

            var totalPoints = points.Sum(p => p.Points);

            return Ok(new
            {
                UserId = userId,
                Points = points,
                Total = totalPoints
            });
        }
        /// <summary>
        /// Obtiene el ranking de usuarios ordenado por puntos totales
        /// </summary>
        [HttpGet("points/ranking")]
        public async Task<IActionResult> GetRanking()
        {
            var ranking = await _context.Users
                .Select(u => new 
                {
                    UserId = u.Id,
                    Username = u.UserName,
                    TotalPoints = _context.LevelPoint
                    .Where(lp => lp.UsuarioId == u.Id)
                    .Select(lp => lp.Points)
                    .Sum()
                })
                .OrderByDescending(u => u.TotalPoints)
                .ToListAsync();

            return Ok(new { dataList = ranking });
        }
    }
}
