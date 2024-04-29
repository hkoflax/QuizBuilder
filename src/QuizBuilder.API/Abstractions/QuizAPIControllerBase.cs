using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizBuilder.Core.Application.Abstractions.Requests;
using System.Security.Claims;

namespace QuizBuilder.API.Abstractions
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class QuizAPIControllerBase : ControllerBase
    {
        private IMediator _mediator;
        protected string UserId;

        /// <summary>
        /// Gets the Mediator.
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        
        /// <summary>
        /// Ensures that an argument is not null.
        /// </summary>
        /// <param name="arg">The argument instance to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        protected static void EnsureArgumentNotNull(object arg, string argumentName)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Handles the request context and returns an appropriate 4xx response or throws an ApiException.
        /// </summary>
        /// <param name="context">The <see cref="RequestContext"/>.</param>
        /// <returns>A <see cref="ObjectResult"/>.</returns>
        /// <exception cref="ApiException">Thrown if the <seealso cref="RequestContext.Status"/> is <seealso cref="RequestStatus.Faulted"/> or <seealso cref="RequestStatus.Cancelled"/>.</exception>
        protected ObjectResult HandleResponse(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Status == RequestStatus.Faulted)
            {
                throw context.Exception;
            }

            if (context.Status == RequestStatus.Cancelled)
            {
                throw new Exception("Unable to process the API request. An internal process was canceled or timed out.");
            }

            return BadRequest(context.Exception is null ? context.Exception : new { Errors = new[] { new { context.Exception.Message } } });
        }


        protected void GetAndCheckUserIdAsync()
        {
            UserId = User.Claims.Where(x => x.Type.Equals(ClaimTypes.NameIdentifier)).FirstOrDefault().Value;

            if (UserId == null || string.IsNullOrWhiteSpace(UserId))
            {
                ModelState.AddModelError("userId", "No valid user id found in claims.");
            }
        }
    }
}
