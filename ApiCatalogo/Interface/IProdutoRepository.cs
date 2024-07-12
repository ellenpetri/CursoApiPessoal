using ApiCatalogo.Models;

namespace ApiCatalogo.Interface;

public interface IProdutoRepository :IRepository<Produto>
{
    IEnumerable<Produto> GetProdutoByCategoria(int id);
}
