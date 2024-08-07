using ApiCatalogo.Context;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

        return produtosOrdenados;
    }
    public IEnumerable<Produto> GetProdutoByCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}