using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Question
{
    public class UpdateQuestionRequest : RequestBase<QuestionDto>, IRequest<Response<UpdateQuestionRequest, QuestionDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateQuestionRequest"/> class.
        /// </summary>
        /// <param name="questionDetails"> A <see cref="QuestionDto"/> that contains the details of the question to update.</param>
        /// <param name="userId">The Id of the user updating the Quizz</param>
        /// <param name="questionId">The Id of the question to update</param>
        public UpdateQuestionRequest(QuestionDto questionDetails, string userId, Guid questionId, int maxAnswerOptionsPerQuestion) : base(userId)
        {
            Details = questionDetails;
            QuestionId = questionId;
            MaxAnswerOptionsPerQuestion = maxAnswerOptionsPerQuestion;
        }

        /// <summary>
        ///  Gets the question id to update.
        /// </summary>
        public Guid QuestionId { get; }

        /// <summary>
        /// Gets the details of the question to use to do the update.
        /// </summary>
        public QuestionDto Details { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(UpdateQuestionRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}