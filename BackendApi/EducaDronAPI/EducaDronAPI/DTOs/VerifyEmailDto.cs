using System.ComponentModel.DataAnnotations;

namespace EducaDronAPI.DTOs
{
    public class VerifyEmailDto
    {
        [Required(ErrorMessage = "El campo email no puede estar vacio.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
