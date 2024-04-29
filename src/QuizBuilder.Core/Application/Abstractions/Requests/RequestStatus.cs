namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    /// <summary>
    /// Represents the status of a request.
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// -1
        /// </summary>
        Cancelled = -1,

        /// <summary>
        /// 0
        /// </summary>
        Completed = 0,

        /// <summary>
        /// 1
        /// </summary>
        Rejected = 1,

        /// <summary>
        /// 2
        /// </summary>
        Faulted = 2,
    }
}
