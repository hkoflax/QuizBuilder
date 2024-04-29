using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests
{
    public static class Queries
    {
        /// <summary>
        /// Get a list of all User quizzes both publish and unpublished.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="IncludeQuestionsDetails">A boolean to wether include the Questions as part of the response or just the header.</param>
        /// <returns>A <see cref="GetUserQuizzes"/> representing a new get quiz user quizzes.</returns>
        public static GetUserQuizzes GetUserQuizzes(string userId, bool IncludeQuestionsDetails = false) 
            => new(userId, IncludeQuestionsDetails);

        /// <summary>
        /// Get a quizz by its ID.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="GetQuiz"/> representing a new get quiz request.</returns>
        public static GetQuiz GetQuiz(string userId, Guid quizId)
            => new(userId, quizId);

        /// <summary>
        /// Get List of vailable quizzes.
        /// </summary>
        /// <param name="pageNumber">The number of page to retrieve</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <returns>A <see cref="GetAvailableQuizzesToTake"/> representing a new get quiz request.</returns>
        public static GetAvailableQuizzesToTake GetAvailableQuizzes(int pageNumber, int pageSize, string userId)
            => new(pageNumber, pageSize, userId);

        /// <summary>
        /// Get a quizz submissions based on ID.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="GetQuizSubmissions"/> representing a new get quiz submissions.</returns>
        public static GetQuizSubmissions GetQuizSubmissions(string userId, Guid quizId)
            => new(userId, quizId);

        /// <summary>
        /// Get a logged In user quiz submission based.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <returns>A <see cref="GetMySubmissions"/> representing a new get submissions.</returns>
        public static GetMySubmissions GetMySubmissions(string userId)
            => new(userId);

        /// <summary>
        /// Get a submission by Id.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="submissionId">The Id of the submission to retrieve.</param>
        /// <returns>A <see cref="GetSubmission"/> representing a new get submissions.</returns>
        public static GetSubmission GetSubmission(string userId, Guid submissionId)
            => new(userId, submissionId);
    }
}
