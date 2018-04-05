using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Services.Interfaces
{
    public interface IBrandService
    {
        Brand GetBrandByKey(string brandKey);
    }
}