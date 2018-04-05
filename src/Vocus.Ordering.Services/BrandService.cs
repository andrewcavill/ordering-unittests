using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;
using Vocus.Ordering.Services.Interfaces;

namespace Vocus.Ordering.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public Brand GetBrandByKey(string brandKey)
        {
            return _brandRepository.GetByUniqueKey(nameof(Brand.BrandKey), brandKey);
        }
    }
}