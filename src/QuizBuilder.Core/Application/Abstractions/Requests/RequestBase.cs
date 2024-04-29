using System.Diagnostics;

namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    /// <summary>
    /// Base class for all the request.
    /// </summary>
    public abstract class RequestBase
    {
        /// <summary>
        /// Gets a unique id that represents the request.
        /// </summary>
        public Guid RequestId { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets the time the request was created represented in <seealso cref="DateTime.Ticks"/>.
        /// </summary>
        public long CreatedAt { get; } = Stopwatch.GetTimestamp();
    }
}
