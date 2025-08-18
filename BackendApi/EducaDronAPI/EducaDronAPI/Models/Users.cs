using Microsoft.AspNetCore.Identity;

namespace EducaDronAPI.Models
{
    public class Users : IdentityUser
    {
        public ICollection<Progress> Progresss { get; set; } = new List<Progress>();
    }
}
