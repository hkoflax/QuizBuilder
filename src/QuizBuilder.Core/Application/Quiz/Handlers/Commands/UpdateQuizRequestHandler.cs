using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class UpdateQuizRequestHandler : IRequestHandler<UpdateQuizRequest, Response<UpdateQuizRequest, QuizDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UpdateQuizRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<UpdateQuizRequest, QuizDto>> Handle(UpdateQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = await _context.Quizzes.Where(x => x.Id == request.QuizId)
                                   .Include(x => x.Author)
                                   .Include(q => q.Questions).ThenInclude(q => q.Answers)
                                   .FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                if (quiz != null && request.UserId == quiz.Author.Id)
                {
                    if (quiz.IsPublished)
                        return request.Faulted<UpdateQuizRequest, QuizDto>(new Exception("You cannot update a published Quiz"));

                    request.Details.SetAnswersWeights();
                    var updatedQuiz = _mapper.Map<QuizBuilder.Domain.Quiz>(request.Details);

                    quiz.Title = updatedQuiz.Title;
                    quiz.Questions = updatedQuiz.Questions;

                    _context.Entry(quiz.Author).State = EntityState.Unchanged;

                    _context.Quizzes.Update(quiz);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    return request.Completed(_mapper.Map<QuizDto>(quiz));
                }

                return request.Failed<UpdateQuizRequest, QuizDto>();
            }
            catch (Exception ex)
            {
                return request.Failed<UpdateQuizRequest, QuizDto>(ex);
            }
        }
    }
}