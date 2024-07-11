using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;

    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produto = _repository.GetProdutos().Take(5).ToList();

        if (produto is null)
            return NotFound("Produtos não encontrados.");

        return Ok(produto);
    }

    [HttpGet("primeiro")]
    public ActionResult<Produto> GetPrimeiro()
    {

        var produto = _repository.GetProdutos().FirstOrDefault();

        if (produto is null)
            return NotFound("Produto não encontrado.");

        return Ok(produto);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.GetProduto(id);

        if (produto is null)
            return NotFound($"Produto com o id {id} não encontrado.");

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Produto não foi informado.");

        _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        bool isAtualizou = _repository.Update(produto);

        if (isAtualizou)
            return Ok(produto);
        else return StatusCode(500, $"Falha ao autualizar produto de id = {id}");
    }

    [HttpDelete("{id:int}'")]
    public ActionResult Delete(int id)
    {
        bool isDeletou = _repository.Delete(id);

        if (isDeletou)
            return Ok($"Produto com id = {id} foi excluido.");

        else return StatusCode(500, $"Falha ao excluir produto de id = {id}");
    }
}