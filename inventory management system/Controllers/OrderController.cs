using AutoMapper;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(id);
            return order == null ? NotFound() : Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);
            await _orderRepo.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, _mapper.Map<OrderDto>(order));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDto dto)
        {
            var existing = await _orderRepo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            _orderRepo.Update(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            _orderRepo.Delete(order);
            return NoContent();
        }
    }

}
