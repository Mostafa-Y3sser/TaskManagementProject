using Microsoft.AspNetCore.Identity;

namespace Task_Management.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public List<Project>? Projects { get; set; }

        public List<Board>? Boards { get; set; }

        public List<TaskActivity>? TaskActivities { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
