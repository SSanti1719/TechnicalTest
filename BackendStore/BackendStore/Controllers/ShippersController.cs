using Domain.Entities;
using Domain.Services.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController : ControllerBase
    {
        private readonly ShipperService _service;

        public ShippersController(ShipperService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shippers = await _service.GetAllAsync();
            return Ok(shippers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shipper = await _service.GetByIdAsync(id);
            if (shipper == null) return NotFound();
            return Ok(shipper);
        }
    }
}
