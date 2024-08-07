using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Interface;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
    {
        var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();

        var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasOrdenados;
    }
}