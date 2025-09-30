using Domain.Entities.Production;

namespace Domain.Interfaces.Repositories.Production
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
    }
}
