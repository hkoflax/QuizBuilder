using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Queries
{
    public class GetMySubmissionsHandler : IRequestHandler<GetMySubmissions, Response<GetMySubmissions, QuizSubmissionDto[]>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetMySubmissionsHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<GetMySubmissions, QuizSubmissionDto[]>> Handle(GetMySubmissions request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var submissions = await _context.QuizSubmissions.Where(x => x.IsSubmitted)
                                                                .Include(x => x.Submittedby)
                                                                .Where(qc => qc.Submittedby.Id == request.UserId)
                                                                .Include(x => x.Quiz)
                                                                .Include(x => x.SelectedResponses)
                                                                  .ThenInclude(x => x.Answer)
                                                                .Include(x => x.SelectedResponses)
                                                                  .ThenInclude(x => x.Question)
                                                                 .ToListAsync();


                return request.Completed(_mapper.Map<QuizSubmissionDto[]>(submissions));
            }
            catch (Exception ex)
            {
                return request.Failed<GetMySubmissions, QuizSubmissionDto[]>(ex);
            }
        }
    }
}
