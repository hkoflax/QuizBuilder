using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Queries
{
    public class GetQuiz : RequestBase<QuizDto>, IRequest<Response<GetQuiz, QuizDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuiz"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        /// <param name="quizId">The Id of the quiz to retrieve</param>
        public GetQuiz(string userId, Guid quizId) : base(userId)
        {
            QuizId = quizId;
        }

        /// <summary>
        ///  Gets the quiz id to retrieve.
        /// </summary>
        public Guid QuizId { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(GetQuiz)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
