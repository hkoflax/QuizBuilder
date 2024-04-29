namespace QuizBuilder.Settings
{
    public class TokenSettings
    {
        public const string configurationSection = "TokenConfiguration";
        public string TokenKey { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}
