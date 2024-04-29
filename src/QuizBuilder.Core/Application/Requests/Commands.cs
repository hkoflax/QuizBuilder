using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Requests.Question;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests
{
    public static class Commands
    {

        /// <summary>
        /// Publish a quiz by its author.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="PublishQuizRequest"/> representing a new publish quiz command.</returns>
        public static PublishQuizRequest PublishQuiz(string userId, Guid quizId, int minAllowedQuestionsPerQuiz)
            => new(userId, quizId, minAllowedQuestionsPerQuiz);

        /// <summary>
        /// create a new quiz.
        /// </summary>
        /// <param name="quizDto">A <see cref="QuizDto"/> object that will be created.</param>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <returns>A <see cref="CreateQuizRequest"/> representing a new create quiz command.</returns>
        public static CreateQuizRequest CreateQuiz(QuizDto quizDto, string userId, int maxAllowedQuestionsPerQuiz, int maxAnswerOptionsPerQuestion)
            => new(quizDto, userId, maxAllowedQuestionsPerQuiz, maxAnswerOptionsPerQuestion);

        /// <summary>
        /// Update a quiz by its author.
        /// </summary>
        /// <param name="quizDto">A <see cref="QuizDto"/> object that will be created.</param>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="UpdateQuizRequest"/> representing a new update quiz command.</returns>
        public static UpdateQuizRequest UpdateQuiz(QuizDto quizDto, string userId, Guid quizId, int maxAllowedQuestionsPerQuiz, int maxAnswerOptionsPerQuestion)
            => new(quizDto, userId, quizId, maxAllowedQuestionsPerQuiz, maxAnswerOptionsPerQuestion);

        /// <summary>
        /// Delete a quiz by its author.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="DeleteQuizRequest"/> representing a new publish quiz command.</returns>
        public static DeleteQuizRequest DeleteQuiz(string userId, Guid quizId)
            => new(userId, quizId);

        /// <summary>
        /// Add question to quiz by its author.
        /// </summary>
        /// <param name="questionDto">A <see cref="QuestionDto"/> object that will be added.</param>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="AddQuestionToQuizRequest"/> representing a new update quiz command.</returns>
        public static AddQuestionToQuizRequest AddQuestionToQuiz(QuestionDto questionDto, string userId, Guid quizId, int maxAnswerOptionsPerQuestion)
            => new(questionDto, userId, quizId, maxAnswerOptionsPerQuestion);

        /// <summary>
        /// Update a question by its author.
        /// </summary>
        /// <param name="quizDto">A <see cref="QuestionDto"/> object that will be used for the update.</param>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to retrieve.</param>
        /// <returns>A <see cref="UpdateQuestionRequest"/> representing a new update quiz command.</returns>
        public static UpdateQuestionRequest UpdateQuestion(QuestionDto questionDto, string userId, Guid questionId, int maxAnswerOptionsPerQuestion)
            => new(questionDto, userId, questionId, maxAnswerOptionsPerQuestion);

        /// <summary>
        /// Delete a question by its author.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="questionId">The Id of the question to retrieve.</param>
        /// <returns>A <see cref="DeleteQuestionRequest"/> representing a new publish quiz command.</returns>
        public static DeleteQuestionRequest DeleteQuestion(string userId, Guid questionId)
            => new(userId, questionId);

        /// <summary>
        /// start a quiz.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="quizId">The Id of the quiz to start.</param>
        /// <returns>A <see cref="StartQuizRequest"/> representing a new publish quiz command.</returns>
        public static StartQuizRequest StartQuiz(string userId, Guid quizId)
            => new(userId, quizId);


        /// <summary>
        /// submit a quiz.
        /// </summary>
        /// <param name="userId">The id of the user that is logged in.</param>
        /// <param name="submissionId">The id of the submission.</param>
        /// <param name="createSubmitModelDto">A <see cref="CreateSubmitDto"/> representing a filled .</param>
        /// <returns>A <see cref="SubmitQuizRequest"/> representing a new submit quiz command.</returns>
        public static SubmitQuizRequest SubmitQuiz(string userId, Guid submissionId, CreateSubmitDto createSubmitModelDto)
            => new(userId, submissionId, createSubmitModelDto);
    }
}
