using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome do produto não foi informado.")]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "A descrição do produto não foi informada.")]
    [StringLength(300)]
    public string? Descricao { get; set; }

    public Decimal Preco { get; set; }

    [Required(ErrorMessage = "A URL da imagem do produto não foi informada.")]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    public int CategoriaId { get; set; }
}