using Domain.Entities.Sales;
using Domain.Interfaces.Repositories.Sales;

namespace Domain.Services.Sales
{
    public class ShipperService
    {
        private readonly IShipperRepository _repository;

        public ShipperService(IShipperRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Shipper>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Shipper> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    }
}
