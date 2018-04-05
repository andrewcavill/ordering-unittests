using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Services.Interfaces
{
    public interface IProductService
    {
        Product GetProductByKey(string productKey);
    }
}