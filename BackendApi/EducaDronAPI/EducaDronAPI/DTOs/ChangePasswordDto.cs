using System.ComponentModel.DataAnnotations;

namespace EducaDronAPI.DTOs
{
    public class ChangePasswordDto
    {

        [Required(ErrorMessage = "El campo email no puede estar vacio.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo contraseña no puede estar vacio.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Ingrese una contraseña entre {2} y {1} caracteres. ")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "El campo confirmar contraseña no puede estar vacio.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
