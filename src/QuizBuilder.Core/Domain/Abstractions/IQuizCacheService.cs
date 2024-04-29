using QuizBuilder.Domain;

namespace QuizBuilder.Core.Domain.Abstractions
{
    public interface IQuizCacheService
    {
        Task AddItemToCacheListAsync(Quiz quiz);
        Task DeleteCacheItemAsync(Guid Id);
        Task<Quiz> GetItemAsync(Guid Id);
        Task<IEnumerable<Quiz>> GetListItemAsync();
    }
}