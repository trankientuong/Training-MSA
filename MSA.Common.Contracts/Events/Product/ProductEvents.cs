namespace MSA.Common.Contracts.Domain.Events.Product;

public record ProductCreated
{
    public Guid ProductId { get; init; }
};

public record ProductValidatedSucceeded
{
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
}

public record ProductValidatedFailed
{
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    public string Reason { get; init; }
}