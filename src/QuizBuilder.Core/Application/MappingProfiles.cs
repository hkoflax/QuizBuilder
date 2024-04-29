using AutoMapper;
using QuizBuilder.Core.Application.Quiz.Mappers;

namespace QuizBuilder.Core.Application
{
    /// <summary>
    /// Provides access to the set of AutoMapper mapping profiles.
    /// </summary>
    public static class MappingProfiles
    {
        /// <summary>
        /// Gets all the AutoMapper mapping profiles in the Maas.Application space.
        /// </summary>
        public static IEnumerable<Profile> All
        {
            get
            {
                yield return new QuizProfile();
                yield return new QuizSubmissionProfile();
            }
        }
    }
}
