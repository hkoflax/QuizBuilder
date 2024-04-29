using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    public class PublishQuizRequest : RequestBase<QuizDto>, IRequest<Response<PublishQuizRequest>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishQuizRequest"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        /// <param name="quizId">The Id of the quiz to publish</param>
        public PublishQuizRequest(string userId, Guid quizId, int minAllowedQuestionsPerQuiz) : base(userId)
        {
            QuizId = quizId;
            MinAllowedQuestionsPerQuiz = minAllowedQuestionsPerQuiz;
        }

        /// <summary>
        ///  Gets the quiz id to publish.
        /// </summary>
        public Guid QuizId { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(PublishQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}