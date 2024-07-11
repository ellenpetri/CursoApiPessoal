using ApiCatalogo.Context;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    //Utilizando o IQueryable fica mais fácil o código ter mais opções de pesquisa depois pois conseguimos aplicar filtros a este retorno.
    public IQueryable<Produto> GetProdutos()
    {
        return _context.Produtos;
    }

    public Produto GetProduto(int id)
    {
        return _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    }

    public Produto Create(Produto produto)
    {
        if (produto is null)
            throw new InvalidOperationException(nameof(produto));

        _context.Produtos.Add(produto);
        _context.SaveChanges();
        return produto;
    }
    public bool Update(Produto produto)
    {
        if (produto is null)
            throw new InvalidOperationException(nameof(produto));

        if (!_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
            return false;

        _context.Produtos.Update(produto);
        _context.SaveChanges();

        return true;
    }
    public bool Delete(int id)
    {
        var produto = _context.Produtos.Find(id);

        if (produto is null)
            return false;

        _context.Remove(produto);
        _context.SaveChanges();
        return true;
    }
}