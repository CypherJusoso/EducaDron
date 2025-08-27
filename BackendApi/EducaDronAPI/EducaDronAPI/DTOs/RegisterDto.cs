
using System.ComponentModel.DataAnnotations;

namespace EducaDronAPI.DTOs
{
    public class RegisterDto
    {

        [Required(ErrorMessage ="El campo username no puede estar vacio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo email no puede estar vacio.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo contraseña no puede estar vacio.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage ="Ingrese una contraseña entre {2} y {1} caracteres. ")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo confirmar contraseña no puede estar vacio.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
