using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class CreateQuizRequestHandler : IRequestHandler<CreateQuizRequest, Response<CreateQuizRequest, QuizDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CreateQuizRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<CreateQuizRequest, QuizDto>> Handle(CreateQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                request.Details.SetAnswersWeights();
                var quiz = _mapper.Map<QuizBuilder.Domain.Quiz>(request.Details);

                quiz.Author = _context.Users.Where(x => x.Id == request.UserId).FirstOrDefault();

                if (quiz.Author != null)
                {
                    _context.Entry(quiz.Author).State = EntityState.Unchanged;
                    await _context.Quizzes.AddAsync(quiz, cancellationToken).ConfigureAwait(false);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    return request.Completed(_mapper.Map<QuizDto>(quiz));
                }

                return request.Faulted<CreateQuizRequest, QuizDto>(new Exception($"User with Id {request.UserId} not found"));
            }
            catch (Exception ex)
            {
                return request.Failed<CreateQuizRequest, QuizDto>(ex);
            }
        }
    }
}
