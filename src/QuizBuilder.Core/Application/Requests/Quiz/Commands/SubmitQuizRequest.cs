using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Commands
{
    public class SubmitQuizRequest : RequestBase<QuizSubmissionDto>, IRequest<Response<SubmitQuizRequest, QuizSubmissionDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitQuizRequest"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user creating the Quizz</param>
        /// <param name="quizSubmissionDto">A <see cref="CreateSubmitDto"/> representing the submitted data</param>
        public SubmitQuizRequest(string userId, Guid submissionId, CreateSubmitDto quizSubmissionDto) : base(userId)
        {
            Details = quizSubmissionDto;
            SubmissionId = submissionId;
        }

        /// <summary>
        ///  Gets quiz submission details.
        /// </summary>
        public CreateSubmitDto Details { get; }

        /// <summary>
        /// The submission Id of the quiz
        /// </summary>
        public Guid SubmissionId { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(SubmitQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
