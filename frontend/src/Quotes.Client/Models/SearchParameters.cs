namespace Quotes.Client.Models;

public record SearchParameters(int limit = 10, int page = 1, string searchQuery = "");
