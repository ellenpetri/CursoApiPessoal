using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome do produto não foi informado.")]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "A descrição do produto não foi informada.")]
    [StringLength(300)]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O preço do produto não foi informado.")]
    [Column(TypeName = "decimal(10,2)")]
    public Decimal Preco { get; set; }

    [Required(ErrorMessage = "A URL da imagem do produto não foi informada.")]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }


    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!String.IsNullOrWhiteSpace(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
                yield return new ValidationResult("A primeira letra do produto deve ser maíscula", [nameof(this.Nome)]);
        }

        if (this.Estoque <= 0)
            yield return new ValidationResult("O estoque deve ser maior que zero", [nameof(this.Estoque)]);


    }

}