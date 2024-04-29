using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Queries
{
    public class GetQuizSubmissionsHandler : IRequestHandler<GetQuizSubmissions, Response<GetQuizSubmissions, QuizSubmissionListDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetQuizSubmissionsHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<GetQuizSubmissions, QuizSubmissionListDto>> Handle(GetQuizSubmissions request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = await _context.Quizzes.Where(x => x.Id == request.QuizId)
                                   .Include(x => x.Author)
                                   .FirstOrDefaultAsync();

                if (quiz != null && quiz.Author.Id == request.UserId)
                {
                    var submissions= await _context.QuizSubmissions.Include(x => x.Quiz)
                                                              .Where(qc => qc.Quiz.Id == request.QuizId)
                                                              .Where(s => s.IsSubmitted)
                                                              .Include(x => x.SelectedResponses)
                                                                .ThenInclude(x => x.Answer)
                                                              .Include(x => x.SelectedResponses)
                                                                .ThenInclude(x => x.Question)
                                                               .ToListAsync();

                    var result = new QuizSubmissionListDto()
                    {
                        CreatedBy = quiz.Author.UserName,
                        QuizId = quiz.Id,
                        Title = quiz.Title,
                        Submissions = _mapper.Map<SubmissiondModelBaseDto[]>(submissions)
                    };

                    return request.Completed(result);
                }
                return request.Failed<GetQuizSubmissions, QuizSubmissionListDto>();
            }
            catch (Exception ex)
            {
                return request.Failed<GetQuizSubmissions, QuizSubmissionListDto>(ex);
            }
        }
    }
}
