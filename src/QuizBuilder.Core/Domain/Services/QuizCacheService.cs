using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Abstractions;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Core.Domain.Settings;
using QuizBuilder.Domain;

namespace QuizBuilder.Core.Domain.Services
{
    public class QuizCacheService : IQuizCacheService
    {
        private readonly ICacheService _cacheService;
        private readonly DataContext _dataContext;
        private readonly CacheSettings _cacheSettings;
        private const string PublishedQuizzesCacheKey = "PublishedQuizzes";

        public QuizCacheService(ICacheService cacheService, DataContext dataContext, IOptions<CacheSettings> cacheSettings)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(dataContext));
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _cacheSettings = cacheSettings?.Value ?? throw new ArgumentNullException(nameof(cacheSettings));
        }

        public async Task<Quiz> GetItemAsync(Guid Id)
        {
            string quizCacheKey = CacheKeys.Quiz(Id);

            if (_cacheService.TryGetValue<Quiz>(quizCacheKey, out var quiz))
            {
                return quiz;
            }

            // If not found in cache, fetch from database
            quiz = await _dataContext.Quizzes.AsNoTracking().Where(x => x.Id == Id)
                                   .Include(x => x.Author)
                                   .Include(q => q.Questions).ThenInclude(q => q.Answers)
                                   .FirstOrDefaultAsync();

            if (quiz != null && quiz.IsPublished)
            {
                // Add to published list and cache the single quiz
                await AddItemToCacheListAsync(quiz).ConfigureAwait(false);
            }

            return quiz;
        }

        public async Task<IEnumerable<Quiz>> GetListItemAsync()
        {
            if (_cacheService.TryGetValue<IEnumerable<Quiz>>(PublishedQuizzesCacheKey, out var quizzes))
            {
                return quizzes;
            }

            // If not found in cache, fetch from database
            quizzes = await _dataContext.Quizzes.AsNoTracking().Where(x => x.IsPublished)
                                   .Include(x => x.Author)
                                   .Include(q => q.Questions).ThenInclude(q => q.Answers)
                                   .ToListAsync();
            // cache the list
            DoSaveList(quizzes);
            return quizzes;
        }

        public async Task AddItemToCacheListAsync(Quiz quiz)
        {
            // Update the list of published quizzes in cache
            var quizzes = await GetListItemAsync().ConfigureAwait(false);
            var newList = new List<Quiz>(quizzes)
            {
                quiz
            };

            // cache the updated list
            DoSaveList(newList);

            // Cache the quiz
            DoCache(CacheKeys.Quiz(quiz.Id), quiz);
        }

        public async Task DeleteCacheItemAsync(Guid Id)
        {
            // Remove the individual quiz from cache if it exists
            string quizCacheKey = CacheKeys.Quiz(Id);
            _cacheService.Remove(quizCacheKey);

            // Update the list of published quizzes in cache
            var quizzes = await GetListItemAsync().ConfigureAwait(false);

            var updateList = new List<Quiz>(quizzes.Where(x => x.Id != Id));

            // cache the updated list
            DoSaveList(updateList);
        }

        private void DoSaveList(IEnumerable<Quiz> newList)
        {
            _cacheService.SetValue(PublishedQuizzesCacheKey, newList, TimeSpan.FromMinutes(_cacheSettings.QuizListCacheTimeInMin));
        }

        private void DoCache<T>(string cacheKey, T data)
        {
            _cacheService.SetValue(cacheKey, data, TimeSpan.FromMinutes(_cacheSettings.QuizCacheTimeInMin));
        }
    }
}