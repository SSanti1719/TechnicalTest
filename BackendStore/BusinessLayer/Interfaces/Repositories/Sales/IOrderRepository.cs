using Domain.DTOs;
using Domain.Entities.Sales;

namespace Domain.Interfaces.Repositories.Sales
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        //Task AddAsync(OrdersDTO order);
        Task<Order> CreateOrderWithDetailsSpAsync(Order order, OrderDetail detail);

        Task<IEnumerable<PredictedOrder>> GetPredictedOrdersAsync();

        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int custId);
    }
}
