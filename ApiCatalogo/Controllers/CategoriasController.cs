using ApiCatalogo.Filters;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(IUnitOfWork unitOfWork, IConfiguration configuration) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet("LerArquivoConfiguracao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];
        var secao1 = _configuration["secao1:chave2"];

        return $"Chave1 = {valor1} || \nChave2 = {valor2} || \nSeção1 => Chave2 = {secao1}";
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        var categoriaProduto = _unitOfWork.CategoriaRepository.GetAll();

        if (categoriaProduto is null)
            return NotFound("Categorias e produtos não encontrados.");

        return Ok(categoriaProduto);
    }

    [HttpGet("primeiro")]
    public ActionResult<Categoria> GetPrimeiro()
    {
        var categoria = _unitOfWork.CategoriaRepository.GetAll().FirstOrDefault();

        if (categoria is null)
            return NotFound("Categoria não encontrada.");

        return Ok(categoria);
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        var categoria = _unitOfWork.CategoriaRepository.GetAll().Take(5);

        if (categoria is null)
            return NotFound("Categorias não encontradas.");

        return Ok(categoria);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound($"Categoria com o id {id} não encontrado.");

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest("Categoria não foi informado.");

        _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound($"Não foi encontrado no banco de dados um categoria com o id {id}");

        var retorno = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        return Ok(retorno);
    }
}
