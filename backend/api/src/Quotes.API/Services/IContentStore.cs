namespace Quotes.API.Services
{
    public interface IContentStore<T>
    {
        Task DeleteAsync(string contentId, CancellationToken token = default);
        Task<T> FetchAsync(string contentId, CancellationToken token = default);
        Task SaveAsync(string contentId, T value, CancellationToken token = default);
    }
}