namespace MSA.OrderService.Services;

public interface IProductService
{
    Task<bool> IsProductExisted(Guid id);
}
