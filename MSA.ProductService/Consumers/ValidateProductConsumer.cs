using MassTransit;
using MSA.Common.Contracts.Domain;
using MSA.Common.Contracts.Domain.Commands.Product;
using MSA.ProductService.Entities;

namespace MSA.ProductService.Consumers;

public class ValidateProductConsumer : IConsumer<ValidateProduct>
{
    private readonly ILogger<ValidateProductConsumer> logger;
    private readonly IRepository<Product> repository;

    public ValidateProductConsumer(
        ILogger<ValidateProductConsumer> logger,
        IRepository<Product> repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Consume(ConsumeContext<ValidateProduct> context)
    {
        var message = context.Message;
        //TODO : Validate and submit Commands/Events for further flow
        logger.Log(LogLevel.Information, 
            $"Receiving message of order {message.OrderId} validating product {message.ProductId}"
        );
    }
}