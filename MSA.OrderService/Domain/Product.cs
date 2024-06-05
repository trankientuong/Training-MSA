using MSA.Common.Contracts.Domain;

namespace MSA.OrderService.Domain;

public class Product : IEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
}