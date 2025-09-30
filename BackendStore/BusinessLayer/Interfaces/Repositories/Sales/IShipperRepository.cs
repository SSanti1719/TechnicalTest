using Domain.Entities.Sales;

namespace Domain.Interfaces.Repositories.Sales
{
    public interface IShipperRepository
    {
        Task<IEnumerable<Shipper>> GetAllAsync();
        Task<Shipper> GetByIdAsync(int id);
    }
}
