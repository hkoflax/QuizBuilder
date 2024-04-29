using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    public class DeleteQuizRequest : RequestBase<QuizDto>, IRequest<Response<DeleteQuizRequest>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteQuizRequest"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        /// <param name="quizId">The Id of the quiz to delete</param>
        public DeleteQuizRequest(string userId, Guid quizId) : base(userId)
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
            return $"{nameof(DeleteQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
