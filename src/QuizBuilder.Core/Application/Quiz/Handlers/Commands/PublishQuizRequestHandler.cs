using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;
using QuizBuilder.Core.Domain.Abstractions;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class PublishQuizRequestHandler : IRequestHandler<PublishQuizRequest, Response<PublishQuizRequest>>
    {
        private readonly DataContext _context;
        private readonly IQuizCacheService _quizCacheService;

        public PublishQuizRequestHandler(DataContext dataContext, IQuizCacheService quizCacheService)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _quizCacheService = quizCacheService ?? throw new ArgumentNullException(nameof(quizCacheService));
        }

        public async Task<Response<PublishQuizRequest>> Handle(PublishQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = _context.Quizzes.Where(x => x.Id == request.QuizId)
                                           .Include(x => x.Author)
                                           .Include(x => x.Questions).ThenInclude(c => c.Answers)
                                           .FirstOrDefault();

                if (quiz != null)
                {

                    if (quiz.Questions.Count < request.MinAllowedQuestionsPerQuiz)
                    {
                        return request.Faulted(new Exception($"Cannot publish a quizz with {quiz.Questions.Count} questions. (Minimum is {request.MinAllowedQuestionsPerQuiz})"));
                    }

                    if (request.UserId == quiz.Author.Id)
                    {
                        if (quiz.IsPublished) return request.Completed();

                        quiz.IsPublished = true;
                        quiz.PublishedDate = DateTime.Now;

                        await _quizCacheService.AddItemToCacheListAsync(quiz).ConfigureAwait(false);

                        _context.Entry(quiz.Author).State = EntityState.Unchanged;
                        foreach (var question in quiz.Questions)
                        {
                            _context.Entry(question).State = EntityState.Unchanged;
                        }
                        _context.Update(quiz);
                        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                        return request.Completed();
                    }
                }

                return request.Failed();
            }
            catch (Exception ex)
            {
                await _quizCacheService.DeleteCacheItemAsync(request.QuizId).ConfigureAwait(false);
                return request.Failed(ex);
            }
        }
    }
}
