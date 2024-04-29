using AutoMapper;
using QuizBuilder.API.Mappings.Quiz;

namespace QuizBuilder.API.Mappings
{
    public static class MappingProfiles
    {
        public static IEnumerable<Profile> ApiProfiles
        {
            get
            {
                yield return new QuizModelProfile();
                yield return new QuizSubmissionModelProfile();
            }
        }
    }
}
