namespace QuizBuilder.Core.Domain.Settings
{
    public class CacheSettings
    {
        public const string configurationSection = "CacheConfiguration";

        public int QuizCacheTimeInMin { get; set; }
        public int QuizListCacheTimeInMin { get; set; }
    }
}
