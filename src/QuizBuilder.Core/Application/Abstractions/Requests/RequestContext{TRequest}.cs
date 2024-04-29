using System.Diagnostics;

namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    /// <summary>
    /// Represents the context of a request.
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    public sealed class RequestContext<TRequest> : RequestContext
        where TRequest : RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestContext{TRequest}"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="completedTicks">The date and time the request completed expressed in <seealso cref="System.DateTime.Ticks"/>.</param>
        /// <param name="status">The <see cref="RequestStatus"/> of the request.</param>
        /// <param name="validationEntries">A collection of <see cref="ValidationEntry"/> objects related to the request.</param>
        /// <param name="exception">A <see cref="System.Exception"/> related to the request.</param>
        internal RequestContext(TRequest request, long completedTicks, RequestStatus status, Exception exception = null)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            CompletedAt = completedTicks;
            Elapsed = GetElapsedTime(request.CreatedAt, completedTicks);
            Status = status;
            Exception = exception;
        }

        private static TimeSpan GetElapsedTime(long createdAt, long completedTicks)
        {
            long elapsedTicks = completedTicks - createdAt;
            double elapsedMilliseconds = (double)elapsedTicks / Stopwatch.Frequency * 1000.0;
            return TimeSpan.FromMilliseconds(elapsedMilliseconds);
        }

        /// <summary>
        /// Gets the request that this context is for.
        /// </summary>
        public TRequest Request { get; }
    }
}
