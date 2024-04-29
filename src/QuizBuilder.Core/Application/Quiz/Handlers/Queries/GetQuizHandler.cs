using AutoMapper;
using MediatR;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;
using QuizBuilder.Core.Domain.Abstractions;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Queries
{
    public class GetQuizHandler : IRequestHandler<GetQuiz, Response<GetQuiz, QuizDto>>
    {
        private readonly IMapper _mapper;
        private readonly IQuizCacheService _quizCacheService;

        public GetQuizHandler(IMapper mapper, IQuizCacheService quizCacheService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _quizCacheService = quizCacheService ?? throw new ArgumentNullException(nameof(quizCacheService));
        }


        public async Task<Response<GetQuiz, QuizDto>> Handle(GetQuiz request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var quiz = await _quizCacheService.GetItemAsync(request.QuizId).ConfigureAwait(false);

                if (quiz != null && (quiz.IsPublished || request.UserId == quiz.Author.Id))
                {
                    return request.Completed(_mapper.Map<QuizDto>(quiz));
                }

                return request.Completed<GetQuiz, QuizDto>(null);
            }
            catch (Exception ex)
            {
                return request.Failed<GetQuiz, QuizDto>(ex);
            }
        }
    }
}
