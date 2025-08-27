using System.ComponentModel.DataAnnotations;

namespace EducaDronAPI.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage ="El campo username no puede estar vacio.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo contraseña no puede estar vacio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
