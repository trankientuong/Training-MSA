namespace MSA.OrderService.Dtos;

public record CreateOrderDto
(
    Guid UserId,
    Guid ProductId
);