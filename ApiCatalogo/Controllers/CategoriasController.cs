using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Interface;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categoria = _unitOfWork.CategoriaRepository.GetCategorias(categoriasParameters);

        var metadata = new
        {
            categoria.TotalCount,
            categoria.PageSize,
            categoria.CurrentPage,
            categoria.TotalPages,
            categoria.HasNext,
            categoria.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriaDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categoria);

        return Ok(categoriaDto);
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll();

        if (categorias == null || !categorias.Any())
            return NotFound("Categorias não encontradas.");

        var categoriaDtos = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriaDtos);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound($"Categoria com o id {id} não encontrado.");

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest("Categoria não foi informado.");

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDto.CategoriaId }, categoriaDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest("Id informado na URL não é igual ao informado no body.");

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound($"Não foi encontrado no banco de dados um categoria com o id {id}");

        var retorno = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDto);
    }
}