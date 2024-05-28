using System.ComponentModel.DataAnnotations;

namespace MSA.ProductService.Dtos
{
    public record ProductDto
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        DateTimeOffset CreatedDate
    );

    public record CreateProductDto
    (
        [Required] string Name,
        [Required] string Description,
        [Range(0,1000)] Decimal Price
    );
}