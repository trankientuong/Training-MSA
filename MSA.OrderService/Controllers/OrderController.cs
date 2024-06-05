using Microsoft.AspNetCore.Mvc;
using MSA.OrderService.Domain;
using MSA.OrderService.Infrastructure.Data;
using MSA.OrderService.Dtos;
using MSA.Common.Contracts.Domain;
using MSA.Common.PostgresMassTransit.PostgresDB;
using MSA.OrderService.Services;
using MassTransit;
using MSA.Common.Contracts.Domain.Commands.Product;

namespace MSA.OrderService.Controllers;

[ApiController]
[Route("v1/order")]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> repository;

    private readonly PostgresUnitOfWork<MainDbContext> uow;
    private readonly IProductService productService;
    private readonly ISendEndpointProvider sendEndpointProvider;


    public OrderController(
        IRepository<Order> repository,
        PostgresUnitOfWork<MainDbContext> uow
,
        IProductService productService,
        ISendEndpointProvider sendEndpointProvider)
    {
        this.repository = repository;
        this.uow = uow;
        this.productService = productService;
        this.sendEndpointProvider = sendEndpointProvider;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetAsync()
    {
        var orders = (await repository.GetAllAsync()).ToList();
        return orders;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostAsync(CreateOrderDto createOrderDto)
    {
        //validate and ensure product exist before creating
        // var isProductExisted = await productService.IsProductExisted(createOrderDto.ProductId);
        // if (!isProductExisted) return BadRequest();

        var order = new Order { 
            Id = Guid.NewGuid(),
            UserId = createOrderDto.UserId,
            OrderStatus = "Order Submitted"
        };
        await repository.CreateAsync(order);

        await uow.SaveChangeAsync();

        //async validate
        var endpoint = await sendEndpointProvider.GetSendEndpoint(
            new Uri("queue:product-validate-product")
        );
        await endpoint.Send(new ValidateProduct
        {
            OrderId = order.Id,
            ProductId = createOrderDto.ProductId
        });

        return CreatedAtAction(nameof(PostAsync), order);
    }
}