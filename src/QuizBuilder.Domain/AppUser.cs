using Microsoft.AspNetCore.Identity;

namespace QuizBuilder.Domain
{
    public class AppUser: IdentityUser
    {
        public string DisplayName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}