using Domain.DTOs;
using Domain.Entities.Sales;
using Domain.Interfaces.Repositories.Sales;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sales
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        private const string PredictedOrdersSql = @"
            WITH O AS (
                SELECT 
                    o.custid,
                    c.companyname as CustomerName,
                    o.orderdate,
                    LAG(o.orderdate) OVER (PARTITION BY o.custid ORDER BY o.orderdate) AS PrevOrderDate
                FROM Sales.Orders o
                JOIN Sales.Customers c ON c.custid = o.custid
            )
            SELECT
                custid,
                CustomerName,
                MAX(orderdate) AS LastOrderDate,
                DATEADD(
                    DAY,
                    FLOOR(
                        1.0 * SUM(CASE WHEN PrevOrderDate IS NULL
                                       THEN 0
                                       ELSE DATEDIFF(DAY, PrevOrderDate, orderdate)
                                  END) / COUNT(*)
                    ),
                    MAX(orderdate)
                ) AS NextPredictedOrder
            FROM O
            GROUP BY custid, CustomerName
            HAVING COUNT(*) > 1
            ORDER BY CustomerName;";

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderWithDetailsSpAsync(Order order, OrderDetail detail)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CustId", (object?)order.CustId ?? DBNull.Value),
                    new SqlParameter("@EmpId", order.EmpId),
                    new SqlParameter("@OrderDate", order.OrderDate),
                    new SqlParameter("@RequiredDate", order.RequiredDate),
                    new SqlParameter("@ShippedDate", (object?)order.ShippedDate ?? DBNull.Value),
                    new SqlParameter("@ShipperId", order.ShipperId),
                    new SqlParameter("@Freight", order.Freight),
                    new SqlParameter("@ShipName", order.ShipName),
                    new SqlParameter("@ShipAddress", order.ShipAddress),
                    new SqlParameter("@ShipCity", order.ShipCity),
                    new SqlParameter("@ShipRegion", (object?)order.ShipRegion ?? DBNull.Value),
                    new SqlParameter("@ShipPostalCode", (object?)order.ShipPostalCode ?? DBNull.Value),
                    new SqlParameter("@ShipCountry", order.ShipCountry),

                    new SqlParameter("@ProductId", detail.ProductId),
                    new SqlParameter("@UnitPrice", detail.UnitPrice),
                    new SqlParameter("@Qty", detail.Qty),
                    new SqlParameter("@Discount", detail.Discount)
                };

                int orderId;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales.usp_CreateOrderWithDetail";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var p in parameters) cmd.Parameters.Add(p);

                    await _context.Database.OpenConnectionAsync();
                    using var reader = await cmd.ExecuteReaderAsync();

                    if (!await reader.ReadAsync())
                    {
                        throw new InvalidOperationException("El SP no devolvió ningún OrderId.");
                    }

                    orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                }

                order.OrderId = orderId;
                return order;
            }
            catch (SqlException ex)
            {
                // Captura errores de SQL Server (FK, CHECK, SP no existe, etc.)
                throw new InvalidOperationException(
                    "Error al ejecutar el SP Sales.usp_CreateOrderWithDetail.", ex);
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error inesperado
                throw new ApplicationException("Error inesperado al crear la orden.", ex);
            }
        }    

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<PredictedOrder>> GetPredictedOrdersAsync()
        {
            return await _context.PredictedOrders
                .FromSqlRaw(PredictedOrdersSql)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int custId)
        {
            return await _context.Orders
                .Where(o => o.CustId == custId)
                .OrderByDescending(o => o.RequiredDate)
                .ToListAsync();
        }    
    }
}
