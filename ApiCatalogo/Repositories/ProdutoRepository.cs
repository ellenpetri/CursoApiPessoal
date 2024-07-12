using ApiCatalogo.Context;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public IEnumerable<Produto> GetProdutoByCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}