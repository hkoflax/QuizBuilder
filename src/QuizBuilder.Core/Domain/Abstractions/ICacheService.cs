namespace QuizBuilder.Core.Domain.Abstractions
{
    public interface ICacheService
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T value, TimeSpan expirationTime);
        bool TryGetValue<T>(string key, out T value);
        void Remove(string key);
    }
}
