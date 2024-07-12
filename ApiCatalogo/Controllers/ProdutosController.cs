using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IProdutoRepository produtoRepository) : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    [HttpGet("ProdutosCategoria/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _produtoRepository.GetProdutoByCategoria(id);

        if (produtos.Count() == 0)
            return NotFound("Produtos não encontrados.");

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produto = _produtoRepository.GetAll().Take(5).ToList();

        if (produto is null)
            return NotFound("Produtos não encontrados.");

        return Ok(produto);
    }

    [HttpGet("First")]
    public ActionResult<Produto> GetPrimeiro()
    {

        var produto = _produtoRepository.GetAll().FirstOrDefault();

        if (produto is null)
            return NotFound("Produto não encontrado.");

        return Ok(produto);
    }

    [HttpGet("ById/{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _produtoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com o id {id} não encontrado.");

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Produto não foi informado.");

        _produtoRepository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        var retorno = _produtoRepository.Update(produto);
        return Ok(produto);
    }

    [HttpDelete("{id:int}'")]
    public ActionResult Delete(int id)
    {
        Produto produto = _produtoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return StatusCode(500, $"Falha ao encontrar produto de id = {id}");

        var retorno = _produtoRepository.Delete(produto);

        return Ok($"Produto com id = {id} foi excluido.");
    }
}