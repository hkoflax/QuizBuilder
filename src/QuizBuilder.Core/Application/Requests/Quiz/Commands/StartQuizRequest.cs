using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    public class StartQuizRequest : RequestBase<QuizSubmissionDto>, IRequest<Response<StartQuizRequest, QuizSubmissionDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartQuizRequest"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        /// <param name="quizId">The Id of the quiz to delete</param>
        public StartQuizRequest(string userId, Guid quizId) : base(userId)
        {
            QuizId = quizId;
        }

        /// <summary>
        ///  Gets the quiz id to delete.
        /// </summary>
        public Guid QuizId { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(StartQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
