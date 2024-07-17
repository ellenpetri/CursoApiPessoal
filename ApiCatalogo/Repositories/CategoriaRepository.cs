using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Interface;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository { }