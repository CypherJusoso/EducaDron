using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducaDronAPI.Models
{
    public class LevelPoint
    {
        [Key]
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int Level { get; set; }
        public int Points { get; set; } = 0;

        [ForeignKey("UsuarioId")]
        public Users Usuario { get; set; }

    }
}
