using ApiCatalogo.Context;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        return GetAll()
            .OrderBy(p => p.Nome)
            .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
            .Take(produtosParams.PageSize).ToList();
    }
    public IEnumerable<Produto> GetProdutoByCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}