using Domain.Entities.Production;
using Domain.Interfaces.Repositories.Production;

namespace Domain.Services.Production
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Product>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Product> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    }
}
