namespace Quotes.API.Models;

public record UrlQueryParameters(int limit = 10, int page = 1, string searchQuery = "");