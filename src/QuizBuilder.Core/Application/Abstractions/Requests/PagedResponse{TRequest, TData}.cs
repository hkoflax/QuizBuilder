using System.Collections;

namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    public class PagedResponse<TRequest, TData> : Response<TRequest, TData>
        where TData : IEnumerable
        where TRequest : PagedRequestBase<TData>
    {
        public PagedResponse(TData data, RequestContext<TRequest> context) : base(data, context)
        {
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

    }
}
