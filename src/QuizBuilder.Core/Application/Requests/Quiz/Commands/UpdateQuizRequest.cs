using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    /// <summary>
    /// Update a quizz;
    /// </summary>
    public class UpdateQuizRequest : RequestBase<QuizDto>, IRequest<Response<UpdateQuizRequest, QuizDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateQuizRequest"/> class.
        /// </summary>
        /// <param name="quizDetails"> A <see cref="QuizDto"/> that contains the details of the quiz to update.</param>
        /// <param name="userId">The Id of the user updating the Quizz</param>
        /// <param name="quizId">The Id of the quiz to update</param>
        public UpdateQuizRequest(QuizDto quizDetails, string userId, Guid quizId, int maxAllowedQuestionsPerQuiz, int maxAnswerOptionsPerQuestion) : base(userId)
        {
            Details = quizDetails;
            QuizId = quizId;
            MaxAllowedQuestionsPerQuiz = maxAllowedQuestionsPerQuiz;
            MaxAnswerOptionsPerQuestion = maxAnswerOptionsPerQuestion;
        }

        /// <summary>
        ///  Gets the quiz id to update.
        /// </summary>
        public Guid QuizId { get; }

        /// <summary>
        /// Gets the details of the quiz to use to do the update.
        /// </summary>
        public QuizDto Details { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(UpdateQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}