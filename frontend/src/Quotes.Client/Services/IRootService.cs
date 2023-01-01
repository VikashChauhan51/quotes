namespace Quotes.Client.Services
{
    public interface IRootService
    {
        Task<IEnumerable<Link>> GetLinksAsync();
    }
}