using System.Diagnostics.Contracts;
namespace MSA.OrderService.Services;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> IsProductExisted(Guid id)
    {
        var result = await httpClient.GetStringAsync($"v1/product/{id}");
        var existedId = Guid.Empty;
        Guid.TryParse(result.Trim('"'), out existedId);
        if (existedId == id) return await Task.FromResult(true);

        return await Task.FromResult(false);
    }
}