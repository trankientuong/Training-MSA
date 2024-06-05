namespace MSA.Common.Contracts.Domain.Events.Product;

public record ProductCreated
{
    public Guid ProductId { get; init; }
};