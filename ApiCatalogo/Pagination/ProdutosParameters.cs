namespace ApiCatalogo.Pagination;

public class ProdutosParameters
{
    private const int _maxPage = 50;
    public int PageNumber { get; set; }
    private int _pageSize;
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