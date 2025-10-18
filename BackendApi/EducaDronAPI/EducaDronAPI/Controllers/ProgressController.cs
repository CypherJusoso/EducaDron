using EducaDronAPI.Data;
using EducaDronAPI.DTOs;
using EducaDronAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducaDronAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProgressController(AppDbContext _context)
        {
            this._context = _context;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProgress(UpdateProgressDto updateProgressDto)
        {
            var levelToUpdate = await _context.Progress.Where(p => p.UsuarioId == updateProgressDto.UserId && p.Nivel == updateProgressDto.LevelNumber).FirstOrDefaultAsync();

            if (levelToUpdate == null)
            {
                return NotFound(new { mensaje = "404 not found." });
            }
            else if (updateProgressDto.NewStatus != "completado" && updateProgressDto.NewStatus != "desbloqueado")
            {
                return BadRequest(new { mensaje = "Debe tener un estado valido" } );
            }

                levelToUpdate.Estado = updateProgressDto.NewStatus;

            if (updateProgressDto.NewStatus == "completado")
            {
                var nextLevel = await _context.Progress
                    .Where(p => p.UsuarioId == updateProgressDto.UserId && p.Nivel == updateProgressDto.LevelNumber +1)
                    .FirstOrDefaultAsync();

                if (nextLevel != null && nextLevel.Estado == "bloqueado") 
                {
                    nextLevel.Estado = "desbloqueado";
                }
            }
            
            await _context.SaveChangesAsync();
            
            return Ok(levelToUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> GetProgress(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("El UserId no puede estar vacio.");
            }

            var progress = await _context.Progress
                .Where(p => p.UsuarioId == userId)
                .OrderBy(p => p.Nivel)
                .ToListAsync();

            var wrapper = new ProgressWrapper { array = progress };

            return Ok(wrapper);
        }
    }

    public class ProgressWrapper
    {
        public List<Progress> array {  get; set; }
    }
}
