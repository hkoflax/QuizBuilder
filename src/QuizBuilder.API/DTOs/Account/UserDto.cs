namespace QuizBuilder.API.DTOs.Account
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
