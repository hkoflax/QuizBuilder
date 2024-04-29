using AutoMapper;
using MediatR;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;
using QuizBuilder.Core.Domain.Abstractions;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Queries
{
    public class GetAvailableQuizzesToTakeHandler : IRequestHandler<GetAvailableQuizzesToTake, PagedResponse<GetAvailableQuizzesToTake, QuizDto[]>>
    {
        private readonly IMapper _mapper;
        private readonly IQuizCacheService _quizCacheService;

        public GetAvailableQuizzesToTakeHandler(IMapper mapper, IQuizCacheService quizCacheService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _quizCacheService = quizCacheService ?? throw new ArgumentNullException(nameof(quizCacheService));
        }

        public async Task<PagedResponse<GetAvailableQuizzesToTake, QuizDto[]>> Handle(GetAvailableQuizzesToTake request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var list = await _quizCacheService.GetListItemAsync().ConfigureAwait(false);

                var result = list.OrderBy(i => i.PublishedDate)
                                 .Skip((request.PageNumber - 1) * request.PageSize)
                                 .Take(request.PageSize).ToList();

                return request.CompletedPaged(_mapper.Map<QuizDto[]>(result), list.Count());
            }
            catch (Exception ex)
            {
                return request.FailedPaged<GetAvailableQuizzesToTake, QuizDto[]>(ex);
            }
        }

    }
}