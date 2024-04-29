using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    public class AddQuestionToQuizRequest : RequestBase<QuizDto>, IRequest<Response<AddQuestionToQuizRequest, QuizDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateQuizRequest"/> class.
        /// </summary>
        /// <param name="quizDetails"> A <see cref="QuestionDto"/> that contains the details of the question to add.</param>
        /// <param name="userId">The Id of the user adding question to the Quiz</param>
        /// <param name="quizId">The Id of the quiz to add the question to.</param>
        public AddQuestionToQuizRequest(QuestionDto quizDetails, string userId, Guid quizId, int maxAnswerOptionsPerQuestion) : base(userId)
        {
            Details = quizDetails;
            QuizId = quizId;
            MaxAnswerOptionsPerQuestion = maxAnswerOptionsPerQuestion;
        }

        /// <summary>
        ///  Gets the quiz id to which the question should be added.
        /// </summary>
        public Guid QuizId { get; }

        /// <summary>
        /// Gets the details of the question to add.
        /// </summary>
        public QuestionDto Details { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(AddQuestionToQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}