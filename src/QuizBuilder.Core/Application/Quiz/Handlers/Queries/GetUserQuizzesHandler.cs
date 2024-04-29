using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Queries
{
    public sealed class GetUserQuizzesHandler : IRequestHandler<GetUserQuizzes, Response<GetUserQuizzes, QuizDto[]>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetUserQuizzesHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<GetUserQuizzes, QuizDto[]>> Handle(GetUserQuizzes request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var query = _context.Quizzes.Include(x => x.Author).AsQueryable();

                if (request.IncludeQuestionsDetails)
                {
                    query = query.Include(q => q.Questions).ThenInclude(q => q.Answers);
                }

                var data = await query.Where(x => x.Author.Id == request.UserId).ToListAsync(cancellationToken).ConfigureAwait(false);

                if (data is null)
                {
                    return request.Completed(Array.Empty<QuizDto>());
                }

                var response = request.Completed(_mapper.Map<QuizDto[]>(data));
                return response;
            }
            catch (Exception ex)
            {
                return request.Failed<GetUserQuizzes, QuizDto[]>(ex);
            }
        }
    }
}
