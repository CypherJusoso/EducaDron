using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducaDronAPI.Models
{
    public class Progress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        public int Nivel {  get; set; }

        [Required]
        [MaxLength(100)]
        public string Estado { get; set; } = "bloqueado";

        [ForeignKey("UsuarioId")]
        public Users Usuario { get; set; } 
    }
}
