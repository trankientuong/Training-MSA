using MSA.ProductService.Entities;

namespace MSA.ProductService.Dtos
{
    public static class Extensions
    {
        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.CreatedDate
            );
        }
    }
}