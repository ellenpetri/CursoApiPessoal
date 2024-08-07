using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Interface;

public interface IProdutoRepository : IRepository<Produto>
{
    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    IEnumerable<Produto> GetProdutoByCategoria(int id);
}