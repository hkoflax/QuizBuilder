using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Quiz.Queries
{
    public class GetMySubmissions : RequestBase<QuizSubmissionDto[]>, IRequest<Response<GetMySubmissions, QuizSubmissionDto[]>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMySubmissions"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        public GetMySubmissions(string userId) : base(userId)
        {
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(StartQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
