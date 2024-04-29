using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Question
{
    public class DeleteQuestionRequest : RequestBase<QuestionDto>, IRequest<Response<DeleteQuestionRequest>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteQuestionRequest"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        /// <param name="questionId">The Id of the question to delete</param>
        public DeleteQuestionRequest(string userId, Guid questionId) : base(userId)
        {
            QuestionId = questionId;
        }

        /// <summary>
        ///  Gets the question id to delete.
        /// </summary>
        public Guid QuestionId { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(DeleteQuestionRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
