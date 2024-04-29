using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Question;

namespace QuizBuilder.Core.Application.Question.Handlers.Commands
{
    public class DeleteQuestionRequestHandler : IRequestHandler<DeleteQuestionRequest, Response<DeleteQuestionRequest>>
    {
        private readonly DataContext _context;

        public DeleteQuestionRequestHandler(DataContext dataContext)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<DeleteQuestionRequest>> Handle(DeleteQuestionRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var question = await _context.Questions
                                         .Include(q => q.Answers)
                                         .Include(q => q.Quiz)
                                         .ThenInclude(q => q.Author)
                                         .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken)
                                         .ConfigureAwait(false);

                if (question != null && request.UserId == question.Quiz.Author.Id)
                {
                    if (question.Quiz.IsPublished)
                        return request.Faulted(new Exception("You cannot delete a question that belongs to a published Quiz"));

                    var previousAnswers = question.Answers;
                    _context.Answers.RemoveRange(previousAnswers);

                    _context.Questions.Remove(question);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    return request.Completed();
                }

                return request.Failed();
            }
            catch (Exception ex)
            {
                return request.Failed(ex);
            }
        }
    }
}
