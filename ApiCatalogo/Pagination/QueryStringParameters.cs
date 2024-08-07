namespace ApiCatalogo.Pagination;

public abstract class QueryStringParameters
{
    private const int _maxPage = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = _maxPage;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > _maxPage) ? _maxPage : value;
        }
    }
}