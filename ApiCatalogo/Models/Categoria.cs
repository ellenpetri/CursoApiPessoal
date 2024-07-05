using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models;

[Table("Categorias")]
public class Categoria
{
    public Categoria() { Produtos = []; }

    [Key]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O nome da categoria não foi informado.")]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "A URL da imagem da categoria não foi informado.")]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    public ICollection<Produto>? Produtos { get; set; }
}