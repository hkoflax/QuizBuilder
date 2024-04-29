using System.Collections;
using System.Diagnostics;

namespace QuizBuilder.Core.Application.Abstractions.Requests
{

    /// <summary>
    /// Extensions on <see cref="RequestBase"/> and <see cref="RequestBase{TData}"/> for creating appropriate <see cref="Response{TRequest}"/> and <see cref="RequestBase{TData}"/> objects.
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Generates a <see cref="RequestStatus.Rejected"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <returns>A <see cref="Response{TRequest}"/>.</returns>
        public static Response<TRequest> Failed<TRequest>(this TRequest request)
            where TRequest : RequestBase
            => new(new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Rejected));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Faulted"/> or <see cref="RequestStatus.Cancelled"/> response depending on the <paramref name="exception"/> type.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="exception">A <see cref="Exception"/>.</param>
        /// <returns>A <see cref="Response{TRequest}"/>.</returns>
        public static Response<TRequest> Failed<TRequest>(this TRequest request, Exception exception)
            where TRequest : RequestBase
            => new(new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), exception is TaskCanceledException ? RequestStatus.Cancelled : RequestStatus.Faulted, exception: exception));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Faulted"/> or <see cref="RequestStatus.Cancelled"/> response depending on the <paramref name="exception"/> type.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="exception">A <see cref="Exception"/>.</param>
        /// <returns>A <see cref="Response{TRequest}"/>.</returns>
        public static Response<TRequest> Faulted<TRequest>(this TRequest request, Exception exception)
            where TRequest : RequestBase
            => new(new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Rejected, exception: exception));


        /// <summary>
        /// Generates a <see cref="RequestStatus.Rejected"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="exception">A <see cref="Exception"/>.</param>
        /// <returns>A <see cref="Response{TRequest, TData}"/>.</returns>
        public static Response<TRequest, TData> Faulted<TRequest, TData>(this TRequest request, Exception exception)
            where TRequest : RequestBase<TData>
            => new(default, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Rejected, exception: exception));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Rejected"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="data">An optional <typeparamref name="TData"/> to include.</param>
        /// <returns>A <see cref="Response{TRequest, TData}"/>.</returns>
        public static Response<TRequest, TData> Failed<TRequest, TData>(this TRequest request, TData data = default)
            where TRequest : RequestBase<TData>
            => new(data, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Rejected));


        /// <summary>
        /// Generates a <see cref="RequestStatus.Faulted"/> or <see cref="RequestStatus.Cancelled"/> response depending on the <paramref name="exception"/> type.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="exception">A <see cref="Exception"/>.</param>
        /// <returns>A <see cref="Response{TRequest, TData}"/>.</returns>
        public static Response<TRequest, TData> Failed<TRequest, TData>(this TRequest request, Exception exception)
            where TRequest : RequestBase<TData>
            => new(default, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), exception is TaskCanceledException ? RequestStatus.Cancelled : RequestStatus.Faulted, exception: exception));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Cancelled"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <returns>A <see cref="Response{TRequest, TData}"/>.</returns>
        public static Response<TRequest, TData> Cancelled<TRequest, TData>(this TRequest request)
            where TRequest : RequestBase<TData>
            => new(default, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Cancelled));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Cancelled"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <returns>A <see cref="Response{TRequest}"/>.</returns>
        public static Response<TRequest> Cancelled<TRequest>(this TRequest request)
            where TRequest : RequestBase
            => new(new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Cancelled));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Completed"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="data">The <typeparamref name="TData"/>.</param>
        /// <returns>A <see cref="Response{TRequest, TData}"/>.</returns>
        public static Response<TRequest, TData> Completed<TRequest, TData>(this TRequest request, TData data)
            where TRequest : RequestBase<TData>
            => new(data, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Completed));

        /// <summary>
        /// Generates a <see cref="RequestStatus.Completed"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="PagedRequestBase{TData}"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="data">The <typeparamref name="TData"/>.</param>
        /// <returns>A <see cref="PagedResponse{TRequest, TData}"/>.</returns>
        public static PagedResponse<TRequest, TData> CompletedPaged<TRequest, TData>(this TRequest request, TData data, int totalCount)
            where TRequest : PagedRequestBase<TData>
            where TData : IEnumerable
            => new(data, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Completed))
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalCount
            };

        /// <summary>
        /// Generates a <see cref="RequestStatus.Faulted"/> or <see cref="RequestStatus.Cancelled"/> response depending on the <paramref name="exception"/> type.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="PagedRequestBase{TData}"/>.</typeparam>
        /// <typeparam name="TData">The type of data returned by the query.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <param name="exception">A <see cref="Exception"/>.</param>
        /// <returns>A <see cref="PagedResponse{TRequest, TData}"/>.</returns>
        public static PagedResponse<TRequest, TData> FailedPaged<TRequest, TData>(this TRequest request, Exception exception)
            where TRequest : PagedRequestBase<TData>
            where TData : IEnumerable
            => new(default, new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), exception is TaskCanceledException ? RequestStatus.Cancelled : RequestStatus.Faulted, exception: exception))
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

        /// <summary>
        /// Generates a <see cref="RequestStatus.Completed"/> response.
        /// </summary>
        /// <typeparam name="TRequest">The type of <see cref="RequestBase"/>.</typeparam>
        /// <param name="request">The <typeparamref name="TRequest"/> instance.</param>
        /// <returns>A <see cref="Response{TRequest}"/>.</returns>
        public static Response<TRequest> Completed<TRequest>(this TRequest request)
            where TRequest : RequestBase
            => new(new RequestContext<TRequest>(request, Stopwatch.GetTimestamp(), RequestStatus.Completed));
    }
}
