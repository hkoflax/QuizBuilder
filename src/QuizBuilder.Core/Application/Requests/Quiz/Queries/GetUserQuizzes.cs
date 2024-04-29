using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Queries
{
    /// <summary>
    /// Represent a request to get a list of all quizzes for a user"
    /// </summary>
    public class GetUserQuizzes : RequestBase<QuizDto[]>, IRequest<Response<GetUserQuizzes, QuizDto[]>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserQuizzes"/> class.
        /// </summary>
        /// <param name="includeQuestions">A boolean to wether get the details about quiz the quizz</param>
        /// <param name="userId">The Id of the quiz to update</param>
        public GetUserQuizzes(string userId, bool includeQuestions = false) : base(userId)
        {
            IncludeQuestionsDetails = includeQuestions;
        }

        /// <summary>
        ///  Gets wether the details about quiz such as question and answer should be include in the response.
        /// </summary>
        public bool IncludeQuestionsDetails { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(GetUserQuizzes)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
