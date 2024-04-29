using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Queries
{
    public class GetSubmissionHandler : IRequestHandler<GetSubmission, Response<GetSubmission, QuizSubmissionDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetSubmissionHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<GetSubmission, QuizSubmissionDto>> Handle(GetSubmission request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var submission = await _context.QuizSubmissions.Where(x => x.Id == request.SubmissionId)
                                                               .Include(x => x.Submittedby)
                                                               .Include(x => x.Quiz)
                                                                .ThenInclude(x => x.Author)
                                                               .Where(qc => qc.Submittedby.Id == request.UserId 
                                                               || qc.Quiz.Author.Id == request.UserId)                                                                
                                                                .Include(x => x.SelectedResponses)
                                                                  .ThenInclude(x => x.Answer)
                                                                .Include(x => x.SelectedResponses)
                                                                  .ThenInclude(x => x.Question)
                                                                 .FirstOrDefaultAsync();


                return request.Completed(_mapper.Map<QuizSubmissionDto>(submission));
            }
            catch (Exception ex)
            {
                return request.Failed<GetSubmission, QuizSubmissionDto>(ex);
            }
        }
    }
}
