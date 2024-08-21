using ApiCatalogo.DTOs;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);

        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltroPreco);

        return ObterProdutos(produtos);
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("ProdutosCategoria/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutoByCategoria(id);

        if (produtos is null || !produtos.Any())
            return NotFound("Produtos não encontrados.");

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produto = _unitOfWork.ProdutoRepository.GetAll().ToList();

        if (produto is null)
            return NotFound("Produtos não encontrados.");

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produto);

        return Ok(produtosDto);
    }

    [HttpGet("First")]
    public ActionResult<ProdutoDTO> GetPrimeiro()
    {
        var produto = _unitOfWork.ProdutoRepository.GetAll().FirstOrDefault();

        if (produto is null)
            return NotFound("Produto não encontrado.");

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
    }

    [HttpGet("ById/{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto com o id {id} não encontrado.");

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produto);

        return Ok(produtosDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest("Produto não foi informado.");

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        produtoDto = _mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto", new { id = produtoDto.ProdutoId }, produtoDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        produtoDto = _mapper.Map<ProdutoDTO>(novoProduto);

        return Ok(produtoDto);
    }

    [HttpDelete("{id:int}'")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        Produto produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return StatusCode(500, $"Falha ao encontrar produto de id = {id}");

        var retorno = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        return Ok($"Produto com id = {id} foi excluido.");
    }
}