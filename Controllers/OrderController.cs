using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Helpers;
using DarazApp.Models;
using DarazApp.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // Create Order (POST)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                // Create Order in the service
                Order orderEntity = await _orderService.CreateOrderAsync(orderDto);

                // Map the created Order entity to OrderDto
                OrderDto orderDtoResult = _mapper.Map<OrderDto>(orderEntity);

                return Ok<OrderDto>(orderDtoResult, OrderResponseMessages.OrderCreatedSuccess);  
            }
            catch (Exception ex)
            {
                return BadRequest<string>(OrderResponseMessages.ErrorOccurred, new List<string> { ex.Message });
            }
        }

        // Get Order by ID (GET)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                // Retrieve order entity by ID
                Order orderEntity = await _orderService.GetOrderByIdAsync(id);

                // Check if the order exists
                if (orderEntity == null)
                {
                    return NotFound<OrderDto>(OrderResponseMessages.OrderNotFound);  // Use constant
                }

                // Map the order entity to OrderDto
                OrderDto orderDtoResult = _mapper.Map<OrderDto>(orderEntity);

                return Ok(orderDtoResult, OrderResponseMessages.OrderRetrievedSuccess);  // Use constant
            }
            catch (Exception ex)
            {
                return BadRequest<string>(OrderResponseMessages.ErrorOccurred, new List<string> { ex.Message });  
            }
        }

        // Get Orders by User (GET)
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(string userId)
        {
            try
            {
                // Retrieve orders for a user
                List<Order> orders = await _orderService.GetOrdersByUserAsync(userId);

                if (orders == null || !orders.Any())
                {
                    return NotFound<OrderDto>(OrderResponseMessages.NoOrdersFoundForUser);  
                }

                // Map list of orders to list of OrderDto
                List<OrderDto> ordersDtoResult = _mapper.Map<List<OrderDto>>(orders);

                return Ok<List<OrderDto>>(ordersDtoResult, OrderResponseMessages.OrdersRetrievedSuccess);  
            }
            catch (Exception ex)
            {
                return BadRequest<string>(OrderResponseMessages.ErrorOccurred, new List<string> { ex.Message });  
            }
        }

        [HttpGet("GetOnPagination")]
        public async Task<ActionResult> GetOrders([FromQuery] PaginationQueryDto paginationQuery)
        {
            try
            {
                PagedResultDto<Order> pagedResult = await _orderService.GetOrdersWithPaginationAsync(paginationQuery);

                if (pagedResult == null || !pagedResult.Items.Any())
                {
                    return NotFound<OrderDto>(OrderResponseMessages.NoOrdersFoundForUser);  // Use constant for "No orders found" message
                }

                // Map the paged result items to OrderDto
                List<OrderDto> orderDtos = _mapper.Map<List<OrderDto>>(pagedResult.Items);

                // Return the paginated result with the pagination metadata
                PagedResultDto<OrderDto> response = new PagedResultDto<OrderDto>
                {
                    Items = orderDtos,
                    TotalRecords = pagedResult.TotalRecords,
                    TotalPages = pagedResult.TotalPages,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                };

                return Ok(response, OrderResponseMessages.OrdersRetrievedSuccess);  
            }
            catch (Exception ex)
            {
                return BadRequest<OrderDto>(OrderResponseMessages.ErrorOccurred, new List<string> { ex.Message });
            }
        }


    }
}



