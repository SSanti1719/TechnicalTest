using Domain.DTOs;
using Domain.Entities.Sales;
using Domain.Interfaces.Repositories.HR;
using Domain.Interfaces.Repositories.Production;
using Domain.Interfaces.Repositories.Sales;


namespace Domain.Services.Sales
{

    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IShipperRepository _shipperRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(
            IOrderRepository orderRepo,
            ICustomerRepository customerRepo,
            IShipperRepository shipperRepo,
            IEmployeeRepository employeeRepo,
            IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _shipperRepo = shipperRepo;
            _employeeRepo = employeeRepo;
            _productRepo = productRepo;
        }

        public async Task<Order> CreateOrderSpAsync(OrderCreateDTO dto)
        {
            // 🔹 Validar existencia del Customer
            if (dto.CustId.HasValue)
            {
                var customer = await _customerRepo.GetByIdAsync(dto.CustId.Value);
                if (customer == null)
                    throw new InvalidOperationException($"El cliente con id {dto.CustId.Value} no existe.");
            }

            // 🔹 Validar existencia del Shipper
            var shipper = await _shipperRepo.GetByIdAsync(dto.ShipperId);
            if (shipper == null)
                throw new InvalidOperationException($"El shipper con id {dto.ShipperId} no existe.");

            var employee = await _employeeRepo.GetByIdAsync(dto.EmpId);
            if (employee == null)
                throw new InvalidOperationException($"El empleado con id {dto.EmpId} no existe.");


            // 🔹 Validar existencia del Product
            var product = await _productRepo.GetByIdAsync(dto.Detail.ProductId);
            if (product == null)
                throw new InvalidOperationException($"El producto con id {dto.Detail.ProductId} no existe.");

            // 🔹 Mapear DTO → Entidad Order
            var order = new Order
            {
                CustId = dto.CustId,
                EmpId = dto.EmpId,
                ShipperId = dto.ShipperId,
                ShipName = dto.ShipName,
                ShipAddress = dto.ShipAddress,
                ShipCity = dto.ShipCity,
                ShipRegion = dto.ShipRegion,
                ShipPostalCode = dto.ShipPostalCode,
                ShipCountry = dto.ShipCountry,
                OrderDate = dto.OrderDate,
                RequiredDate = dto.RequiredDate,
                ShippedDate = dto.ShippedDate,
                Freight = dto.Freight
            };

            // 🔹 Mapear detalle
            var detail = new OrderDetail
            {
                ProductId = dto.Detail.ProductId,
                UnitPrice = dto.Detail.UnitPrice,
                Qty = dto.Detail.Qty,
                Discount = dto.Detail.Discount
            };

            // 🔹 Llamar al repositorio principal (SP en SQL Server)
            return await _orderRepo.CreateOrderWithDetailsSpAsync(order, detail);
        }

        public Task<IEnumerable<Order>> GetAllAsync() => _orderRepo.GetAllAsync();


        public Task<IEnumerable<PredictedOrder>> GetPredictedOrdersAsync()
        {
            return _orderRepo.GetPredictedOrdersAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int custId)
        {

            var exists = await _customerRepo.GetByIdAsync(custId);
            if (exists == null)
                throw new ArgumentException($"Customer with ID {custId} does not exist.");

            return await _orderRepo.GetOrdersByCustomerAsync(custId);
        }
    }

}

