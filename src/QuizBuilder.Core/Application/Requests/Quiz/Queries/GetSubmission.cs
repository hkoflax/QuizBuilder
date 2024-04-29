using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Quiz.Queries
{
    public class GetSubmission : RequestBase<QuizSubmissionDto>, IRequest<Response<GetSubmission, QuizSubmissionDto>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubmission"/> class.
        /// </summary>
        /// <param name="userId">The Id of the user retrieving the submission.</param>
        /// <param name="submissionId">The Id of the submission to retrieve.</param>
        public GetSubmission(string userId, Guid submissionId) : base(userId)
        {
            SubmissionId = submissionId;
        }

        /// <summary>
        ///  Gets the submission id to retrieve.
        /// </summary>
        public Guid SubmissionId { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(StartQuizRequest)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
