namespace EducaDronAPI.DTOs
{
    public class UpdateProgressDto
    {
        public string UserId { get; set; }
        public int LevelNumber { get; set; }
        public string NewStatus { get; set; }
    }
}
