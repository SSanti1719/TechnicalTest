using Domain.Entities.Sales;
using Domain.Interfaces.Repositories.Sales;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sales
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly AppDbContext _context;

        public ShipperRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipper>> GetAllAsync()
        {
            return await _context.Shippers.ToListAsync();
        }

        public async Task<Shipper> GetByIdAsync(int id)
        {
            return await _context.Shippers.FindAsync(id);
        }
    }
}
