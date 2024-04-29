using Newtonsoft.Json;

namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    /// <summary>
    /// Represents the base type of response to a request.
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    public class Response<TRequest>
        where TRequest : RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Response{TRequest}"/> class.
        /// </summary>
        /// <param name="context">The associated <see cref="RequestContext{TRequest}"/>.</param>
        public Response(RequestContext<TRequest> context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets a value indicating whether the response represents a successful completion of the request.
        /// </summary>
        public bool Succeeded => Context?.Status == RequestStatus.Completed;

        /// <summary>
        /// Gets the request context.
        /// </summary>
        public RequestContext<TRequest> Context { get; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"RequestId-{Context.Request.RequestId} => {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
