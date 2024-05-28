using MSA.Common.Contracts.Domain;

namespace MSA.OrderService.Domain
{
    public class OrderDetail : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
    }
}