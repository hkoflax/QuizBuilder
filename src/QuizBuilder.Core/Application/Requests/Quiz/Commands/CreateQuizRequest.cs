using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    /// <summary>
    /// Represent a command to create a quizz"
    /// </summary>
    public class CreateQuizRequest : RequestBase<QuizDto>, IRequest<Response<CreateQuizRequest, QuizDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuizRequest"/> class.
        /// </summary>
        /// <param name="quizDetails"> A <see cref="QuizDto"/> that contains the details of the quiz to create.</param>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        public CreateQuizRequest(QuizDto quizDetails, string userId, int maxAllowedQuestionsPerQuiz, int maxAnswerOptionsPerQuestion) : base(userId)
        {
            Details = quizDetails;
            MaxAllowedQuestionsPerQuiz = maxAllowedQuestionsPerQuiz;
            MaxAnswerOptionsPerQuestion = maxAnswerOptionsPerQuestion;
        }

        /// <summary>
        /// Gets the details of the quiz to create.
        /// </summary>
        public QuizDto Details { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(CreateQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
