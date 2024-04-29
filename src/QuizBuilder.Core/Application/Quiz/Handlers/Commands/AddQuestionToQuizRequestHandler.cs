using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class AddQuestionToQuizRequestHandler : IRequestHandler<AddQuestionToQuizRequest, Response<AddQuestionToQuizRequest, QuizDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AddQuestionToQuizRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<AddQuestionToQuizRequest, QuizDto>> Handle(AddQuestionToQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = await _context.Quizzes.Where(x => x.Id == request.QuizId)
                                   .Include(x => x.Author)
                                   .Include(q => q.Questions).ThenInclude(q => q.Answers)
                                   .FirstOrDefaultAsync();

                if (quiz != null && request.UserId == quiz.Author.Id)
                {
                    if (quiz.IsPublished)
                        return request.Faulted<AddQuestionToQuizRequest, QuizDto>(new Exception("You cannot update a published Quiz"));
                    
                    if (quiz.Questions.Count == 10)
                        return request.Faulted<AddQuestionToQuizRequest, QuizDto>(new Exception("You already have 10 questions in this quizz"));

                    request.Details.SetAnswersWeights();
                    var newQuestion = _mapper.Map<QuizBuilder.Domain.Question>(request.Details);

                    quiz.Questions.Add(newQuestion);

                    _context.Quizzes.Update(quiz);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    return request.Completed(_mapper.Map<QuizDto>(quiz));
                }

                return request.Failed<AddQuestionToQuizRequest, QuizDto>();
            }
            catch (Exception ex)
            {
                return request.Failed<AddQuestionToQuizRequest, QuizDto>(ex);
            }
        }
    }
}