using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        var produto = await _context.Produtos.AsNoTracking().Take(5).ToListAsync();

        if (produto is null)
            return NotFound("Produtos não encontrados.");

        return produto;
    }

    [HttpGet("primeiro")]
    public async Task<ActionResult<Produto>> GetPrimeiro()
    {

        var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync();

        if (produto is null)
            return NotFound("Produto não encontrado.");

        return produto;
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> Get(int id)
    {
        var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com o id {id} não encontrado.");

        return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Produto não foi informado.");

        _context.Produtos.Add(produto);

        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        _context.Entry(produto).State = EntityState.Modified;

        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}'")]
    public ActionResult Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Não foi encontrado no banco de dados um produto com o id {id}");

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return Ok(produto);
    }
}
