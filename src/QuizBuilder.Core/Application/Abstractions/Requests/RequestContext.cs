namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    /// <summary>
    /// Represents the base request context.
    /// </summary>
    public abstract class RequestContext
    {
        /// <summary>
        /// Gets or sets the time the request was completed represented in <seealso cref="System.DateTime.Ticks"/>.
        /// </summary>
        public long CompletedAt { get; protected set; }

        /// <summary>
        /// Gets or sets the elapsed time of the request.
        /// </summary>
        public TimeSpan Elapsed { get; protected set; }

        /// <summary>
        /// Gets or sets the status of the request.
        /// </summary>
        public RequestStatus Status { get; protected set; }

        /// <summary>
        /// Gets or sets an exception that occurred during the request.
        /// </summary>
        public Exception Exception { get; protected set; }
    }
}
