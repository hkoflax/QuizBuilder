using MediatR;
using Microsoft.Extensions.Logging;

namespace QuizBuilder.Core.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        /// <summary>
        /// Inintialise a <see cref="LoggingBehaviour{TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger{TCategoryName}"/> of <see cref="LoggingBehaviour{TRequest, TResponse}"/>.</param>
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.ToString());

            var response = await next();

            _logger.LogInformation($"Handled: {response}");
            return response;
        }
    }
}
