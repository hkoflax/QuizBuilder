using System.Collections;

namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    public abstract class PagedRequestBase<TData> : RequestBase<TData>
           where TData : IEnumerable
    {
        public PagedRequestBase(string userId) : base(userId) { }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
