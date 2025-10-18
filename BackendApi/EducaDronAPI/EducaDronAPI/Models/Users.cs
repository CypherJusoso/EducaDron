using Microsoft.AspNetCore.Identity;

namespace EducaDronAPI.Models
{
    public class Users : IdentityUser
    {
        public ICollection<Progress> Progresss { get; set; } = new List<Progress>();
        public ICollection<LevelPoint> LevelPoints { get; set; } = new List<LevelPoint>();
    }
}
