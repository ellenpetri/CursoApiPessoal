using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Interface;

public interface ICategoriaRepository : IRepository<Categoria> 
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParams);
}