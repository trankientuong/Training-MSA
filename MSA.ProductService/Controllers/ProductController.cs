using Microsoft.AspNetCore.Mvc;
using MSA.ProductService.Dtos;
using MSA.ProductService.Entities;
using MSA.Common.Contracts.Domain;

namespace MSA.ProductService.Controllers
{
    [ApiController]
    [Route("v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductController(
            IRepository<Product> repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAsync()
        {
            var products = (await _repository.GetAllAsync())
                            .Select(p => p.AsDto());
            return products;
        }

        //Get v1/product/123
        [HttpGet("{id}")]
        public async Task<ActionResult<Guid>> GetByIdAsync(Guid id)
        {
            if (id == null) return BadRequest();

            var product = (await _repository.GetAsync(id));
            if (product == null) return Ok(Guid.Empty);

            return Ok(product.Id);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostAsync(
            CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Id = new Guid(),
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _repository.CreateAsync(product);

            return CreatedAtAction(nameof(PostAsync), product.AsDto());
        }
    }
}