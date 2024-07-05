using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public readonly ILogger _logger;

    public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("LerArquivoConfiguracao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];
        var secao1 = _configuration["secao1:chave2"];

        return $"Chave1 = {valor1} || \nChave2 = {valor2} || \nSeção1 => Chave2 = {secao1}";
    }


    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
    {
        try
        {
            _logger.LogInformation("--------------------------GetCategoriasProdutos----------------------------------");
            //Não devemos retornar tudo de uma unica vez
            //var categoriaProduto = _context.Categorias.AsNoTracking().Include(P => P.Produtos).ToList();
            var categoriaProduto = await _context.Categorias.AsNoTracking().Include(P => P.Produtos).Where(c => c.CategoriaId <= 5).ToListAsync();

            if (categoriaProduto is null)
                return NotFound("Categorias e produtos não encontrados.");

            return categoriaProduto;
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpGet("primeiro")]
    public async Task<ActionResult<Categoria>> GetPrimeiro()
    {
        try
        {
            //Não devemos retornar tudo de uma unica vez
            //var categoria = _context.Categorias.AsNoTracking().ToList();
            var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync();

            if (categoria is null)
                return NotFound("Categoria não encontrada.");

            return categoria;
        }
        catch 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        try
        {
            //Não devemos retornar tudo de uma unica vez
            //var categoria = _context.Categorias.AsNoTracking().ToList();
            var categoria = await _context.Categorias.AsNoTracking().Take(5).ToListAsync();

            if (categoria is null)
                return NotFound("Categorias não encontradas.");

            return categoria;
        }
        catch 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        try
        {
            var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Categoria com o id {id} não encontrado.");

            return categoria;
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        try
        {
            if (categoria is null)
                return BadRequest("Categoria não foi informado.");

            _context.Categorias.Add(categoria);

            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        try
        {
            if (id != categoria.CategoriaId)
                return BadRequest("Id informado na URL não é igual ao informado no body.");

            _context.Entry(categoria).State = EntityState.Modified;

            _context.SaveChanges();

            return Ok(categoria);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Não foi encontrado no banco de dados um categoria com o id {id}");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }
}
