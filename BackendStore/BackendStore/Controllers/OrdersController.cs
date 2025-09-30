using Domain.DTOs;
using Domain.Entities;
using Domain.Services.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpPost("with-sp")]
        public async Task<IActionResult> CreateWithSp(OrderCreateDTO dto)
        {
            try
            {
                var order = await _service.CreateOrderSpAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
            }
            catch (InvalidOperationException ex)
            {
                // Errores de negocio / SP no devuelve OrderId / violaciones de integridad
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (ApplicationException ex)
            {
                // Errores inesperados internos (ej: conexión perdida)
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Fallback por si algo no previsto ocurre
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado.",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            // Para demo solo devolvemos el id
            return Ok(new { OrderId = id });
        }

        [HttpGet("predicted")]
        public async Task<IActionResult> GetPredictedOrders()
        {
            var result = await _service.GetPredictedOrdersAsync();
            return Ok(result);
        }

        [HttpGet("by-customer/{custId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int custId)
        {
            var orders = await _service.GetOrdersByCustomerAsync(custId);

            var result = orders.Select(o => new
            {
                o.OrderId,
                o.RequiredDate,
                o.ShippedDate,
                o.ShipName,
                o.ShipAddress,
                o.ShipCity
            });

            return Ok(result);
        }
    }
}
