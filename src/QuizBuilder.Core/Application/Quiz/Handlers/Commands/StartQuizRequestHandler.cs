using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;
using QuizBuilder.Domain;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class StartQuizRequestHandler : IRequestHandler<StartQuizRequest, Response<StartQuizRequest, QuizSubmissionDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public StartQuizRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<StartQuizRequest, QuizSubmissionDto>> Handle(StartQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = await _context.Quizzes.Where(x => x.Id == request.QuizId)
                                   .Include(x => x.Author)
                                   .Include(q => q.Questions).ThenInclude(q => q.Answers)
                                   .FirstOrDefaultAsync();

                if (quiz != null)
                {
                    if (!quiz.IsPublished) 
                        return request.Faulted<StartQuizRequest, QuizSubmissionDto>(new Exception("You cannot take an unpublished Quiz"));

                    var createdBy = _context.Users.Where(x => x.Id == request.UserId).FirstOrDefault();

                    var newSubmission = new QuizSubmission()
                    {
                        Quiz = quiz,
                        QuizTitle = quiz.Title,
                        CreatedAt = DateTime.Now,
                        Submittedby = createdBy
                    };

                    _context.Entry(quiz).State = EntityState.Unchanged;
                    _context.Entry(newSubmission.Submittedby).State = EntityState.Unchanged;

                    await _context.QuizSubmissions.AddAsync(newSubmission);
                    await _context.SaveChangesAsync();

                    return request.Completed(_mapper.Map<QuizSubmissionDto>(newSubmission)); 
                }

                return request.Failed<StartQuizRequest, QuizSubmissionDto>();
            }
            catch (Exception ex)
            {
                return request.Failed<StartQuizRequest, QuizSubmissionDto>(ex);
            }
        }
    }
}
