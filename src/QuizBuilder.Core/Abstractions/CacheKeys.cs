using System.Reflection;

namespace QuizBuilder.Core.Abstractions
{
    public class CacheKeys
    {
        private static string _assembly = Assembly.GetEntryAssembly().GetName().Name.Replace('.', '_');

        public static string Quiz(Guid quizId)
        {
            if (quizId == Guid.Empty)
            {
                throw new ArgumentException("thw quiz Id missing.", nameof(quizId));
            }

            return GenerateCacheKey("Quiz_", quizId);
        }

        private static string GenerateCacheKey(string prefix, Guid id) => $"{prefix}_{id}";
    }
}
