using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;
using QuizBuilder.Core.Domain.Abstractions;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class DeleteQuizRequestHandler : IRequestHandler<DeleteQuizRequest, Response<DeleteQuizRequest>>
    {
        private readonly DataContext _context;
        private readonly IQuizCacheService _quizCacheService;

        public DeleteQuizRequestHandler(DataContext dataContext, IQuizCacheService quizCacheService)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _quizCacheService = quizCacheService ?? throw new ArgumentNullException(nameof(quizCacheService));
        }

        public async Task<Response<DeleteQuizRequest>> Handle(DeleteQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = _context.Quizzes.Where(x => x.Id == request.QuizId)
                                           .Include(x => x.Author)
                                           .Include(x => x.Questions)
                                           .FirstOrDefault();

                if (quiz != null && request.UserId == quiz.Author.Id)
                {
                    _context.Quizzes.Remove(quiz);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    if (quiz.IsPublished)
                    {
                        await _quizCacheService.DeleteCacheItemAsync(request.QuizId).ConfigureAwait(false);
                    }

                    return request.Completed();
                }

                return request.Failed();
            }
            catch (DbUpdateException)
            {
                return request.Faulted(new Exception("You are unable to delete this quiz as there have been submissions. However, we can archive it to prevent any further submissions."));
            }
            catch (Exception ex)
            {
                return request.Failed(ex);
            }
        }
    }
}
