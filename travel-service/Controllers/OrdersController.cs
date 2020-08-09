using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using travel_service.Dtos;
using travel_service.Services;

namespace travel_service.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        public OrdersController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders()
        {
            // get userid
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orders = await _touristRouteRepository.GetOrdersByUserId(userId);
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }
        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            var userId = _httpContextAccessor
               .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var order = await _touristRouteRepository.GetOrderById(orderId);
            return Ok(_mapper.Map<OrderDto>(order));
        }

    }
}
