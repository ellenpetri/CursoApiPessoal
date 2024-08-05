using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet("ProdutosCategoria/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutoByCategoria(id);

        if (produtos.Count() == 0)
            return NotFound("Produtos não encontrados.");

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produto = _unitOfWork.ProdutoRepository.GetAll().Take(5).ToList();

        if (produto is null)
            return NotFound("Produtos não encontrados.");

        return Ok(produto);
    }

    [HttpGet("First")]
    public ActionResult<Produto> GetPrimeiro()
    {

        var produto = _unitOfWork.ProdutoRepository.GetAll().FirstOrDefault();

        if (produto is null)
            return NotFound("Produto não encontrado.");

        return Ok(produto);
    }

    [HttpGet("ById/{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com o id {id} não encontrado.");

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Produto não foi informado.");

        _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        var retorno = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(produto);
    }

    [HttpDelete("{id:int}'")]
    public ActionResult Delete(int id)
    {
        Produto produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return StatusCode(500, $"Falha ao encontrar produto de id = {id}");

        var retorno = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        return Ok($"Produto com id = {id} foi excluido.");
    }
}