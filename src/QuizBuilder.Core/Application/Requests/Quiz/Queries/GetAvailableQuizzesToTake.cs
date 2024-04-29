using MediatR;
using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;

namespace QuizBuilder.Core.Application.Requests.Quiz.Queries
{
    public class GetAvailableQuizzesToTake : PagedRequestBase<QuizDto[]>, IRequest<PagedResponse<GetAvailableQuizzesToTake, QuizDto[]>>
    {
        public GetAvailableQuizzesToTake(int pageNumber, int pageSize, string userId) : base(userId)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(GetAvailableQuizzesToTake)}(RequestId-{RequestId}) => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
