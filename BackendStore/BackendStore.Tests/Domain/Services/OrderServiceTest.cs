using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities.HR;
using Domain.Entities.Production;
using Domain.Entities.Sales;
using Domain.Interfaces.Repositories.HR;
using Domain.Interfaces.Repositories.Production;
using Domain.Interfaces.Repositories.Sales;
using Domain.Services.Sales;
using FluentAssertions;
using Moq;
using Xunit;

namespace BackendStore.Tests.Domain.Services.Sales
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<ICustomerRepository> _customerRepoMock;
        private readonly Mock<IShipperRepository> _shipperRepoMock;
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;

        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _shipperRepoMock = new Mock<IShipperRepository>();
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _productRepoMock = new Mock<IProductRepository>();

            _service = new OrderService(
                _orderRepoMock.Object,
                _customerRepoMock.Object,
                _shipperRepoMock.Object,
                _employeeRepoMock.Object,
                _productRepoMock.Object
            );
        }

        [Fact]
        public async Task CreateOrderSpAsync_ShouldThrow_WhenCustomerDoesNotExist()
        {
            var dto = BuildOrderDto();
            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustId.Value)).ReturnsAsync((Customer?)null);

            Func<Task> act = async () => await _service.CreateOrderSpAsync(dto);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"El cliente con id {dto.CustId.Value} no existe.");
        }

        [Fact]
        public async Task CreateOrderSpAsync_ShouldThrow_WhenShipperDoesNotExist()
        {
            var dto = BuildOrderDto();
            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustId.Value)).ReturnsAsync(new Customer());
            _shipperRepoMock.Setup(r => r.GetByIdAsync(dto.ShipperId)).ReturnsAsync((Shipper?)null);

            Func<Task> act = async () => await _service.CreateOrderSpAsync(dto);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"El shipper con id {dto.ShipperId} no existe.");
        }

        [Fact]
        public async Task CreateOrderSpAsync_ShouldReturnOrder_WhenAllEntitiesExist()
        {
            var dto = BuildOrderDto();
            _customerRepoMock.Setup(r => r.GetByIdAsync(dto.CustId.Value)).ReturnsAsync(new Customer());
            _shipperRepoMock.Setup(r => r.GetByIdAsync(dto.ShipperId)).ReturnsAsync(new Shipper());
            _employeeRepoMock.Setup(r => r.GetByIdAsync(dto.EmpId)).ReturnsAsync(new Employee());
            _productRepoMock.Setup(r => r.GetByIdAsync(dto.Detail.ProductId)).ReturnsAsync(new Product());

            var expectedOrder = new Order { OrderId = 1, CustId = dto.CustId };
            _orderRepoMock.Setup(r => r.CreateOrderWithDetailsSpAsync(It.IsAny<Order>(), It.IsAny<OrderDetail>()))
                          .ReturnsAsync(expectedOrder);

            var result = await _service.CreateOrderSpAsync(dto);

            result.Should().NotBeNull();
            result.OrderId.Should().Be(1);
        }

        [Fact]
        public async Task GetOrdersByCustomerAsync_ShouldThrow_WhenCustomerDoesNotExist()
        {
            int custId = 85;
            _customerRepoMock.Setup(r => r.GetByIdAsync(custId)).ReturnsAsync((Customer?)null);

            Func<Task> act = async () => await _service.GetOrdersByCustomerAsync(custId);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"Customer with ID {custId} does not exist.");
        }

        [Fact]
        public async Task GetOrdersByCustomerAsync_ShouldReturnOrders_WhenCustomerExists()
        {
            int custId = 85;
            _customerRepoMock.Setup(r => r.GetByIdAsync(custId)).ReturnsAsync(new Customer());
            _orderRepoMock.Setup(r => r.GetOrdersByCustomerAsync(custId))
                          .ReturnsAsync(new List<Order> { new Order { OrderId = 10, CustId = custId } });

            var result = await _service.GetOrdersByCustomerAsync(custId);

            result.Should().ContainSingle();
            result.First().OrderId.Should().Be(10);
        }

        // 🔹 Helper para crear un DTO de prueba
        private static OrderCreateDTO BuildOrderDto()
        {
            return new OrderCreateDTO
            {
                CustId = 1,
                EmpId = 2,
                ShipperId = 3,
                ShipName = "Test",
                ShipAddress = "Street 123",
                ShipCity = "City",
                ShipCountry = "Country",
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7),
                ShippedDate = null,
                Freight = 10,
                Detail = new OrderDetailDto
                {
                    ProductId = 5,
                    UnitPrice = 100,
                    Qty = 2,
                    Discount = 0
                }
            };
        }
    }
}
