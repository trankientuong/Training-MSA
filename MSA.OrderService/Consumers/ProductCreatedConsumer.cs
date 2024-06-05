using MassTransit;
using MSA.Common.Contracts.Domain;
using MSA.Common.Contracts.Domain.Events.Product;
using MSA.Common.PostgresMassTransit.PostgresDB;
using MSA.OrderService.Domain;
using MSA.OrderService.Infrastructure.Data;

namespace MSA.OrderService.Consumers;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly IRepository<Product> productRepository;
    private readonly PostgresUnitOfWork<MainDbContext> uoW;

    public ProductCreatedConsumer(
        IRepository<Product> productRepository,
        PostgresUnitOfWork<MainDbContext> uoW)
    {
        this.productRepository = productRepository;
        this.uoW = uoW;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        var message = context.Message;
        Product product = new Product {
            Id = new Guid(),
            ProductId = message.ProductId
        };
        await productRepository.CreateAsync(product);
        await uoW.SaveChangeAsync();
    }
}