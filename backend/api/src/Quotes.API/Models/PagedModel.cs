namespace Quotes.API.Models;

public record PagedModel<TModel>
{
    const int MaxPageSize = 10;
    private int _pageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => (CurrentPage > 1);
    public bool HasNext => (CurrentPage < TotalPages);
    public IList<TModel> Items { get; set; }

    public PagedModel()
    {
        Items = new List<TModel>();
    }
}
