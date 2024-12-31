using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Repositories;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        private int? GetUserIdFromQuery()
        {
            var userIdQuery = HttpContext.Request.Query["userId"].FirstOrDefault();
            return int.TryParse(userIdQuery, out int userId) ? userId : (int?)null;
        }
        [HttpPost("addorderfromcart")]
        public async Task<IActionResult> AddOrderFromCart([FromBody] OrderDto placeOrderDto, int userId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            if (userId == 0) return Unauthorized("Invalid user.");

            var cartItems = await unitOfWork.CartRepository.GetAllItemsFromCart(userId);

            if (!cartItems.Any())
            {
                return BadRequest("The cart is empty.");
            }

            var orderDetails = cartItems.Select(cartItem => new OrderDetail
            {
                ItemId = cartItem.ItemId,
                Quantity = (int)cartItem.Quantity,
                Price = (decimal)cartItem.price
            }).ToList();
            var orderDetailsDto = ConvertOrderDetailsToDto(orderDetails);
            var orderDto = new OrderDto
            {
                AddressLine1 = placeOrderDto.AddressLine1,
                City = placeOrderDto.City,
                State = placeOrderDto.State,
                PostalCode = placeOrderDto.PostalCode,
                Country = placeOrderDto.Country,
                OrderDetails = orderDetailsDto  
            };
            var order = MapOrderDtoToOrder(orderDto, userId);
            order.CalculateTotalAmount(); 
            var createdOrder = await unitOfWork.OrderRepository.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }


        [HttpGet("getwithid")]
        public async Task<IActionResult> GetOrderById([FromQuery] int orderId, [FromQuery] int userId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            if (userId == 0) return Unauthorized("Invalid user.");

            var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(orderId, userId);
            if (order == null) return NotFound("Order not found or unauthorized.");

            order.CalculateTotalAmount();  

            return Ok(order);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetOrdersByUser([FromQuery] int userId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            if (userId == 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var orders = await unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);

             

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromQuery] int orderId, [FromQuery] int userId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            if (userId == 0) return Unauthorized("Invalid user.");

            var isDeleted = await unitOfWork.OrderRepository.DeleteOrderAsync(orderId, userId);
            if (!isDeleted) return NotFound("Order not found or unauthorized.");

            return NoContent();
        }

        private List<OrderDetail> ConvertCartItemsToOrderDetails(IEnumerable<ShoppingCartItems> cartItems)
        {
            return cartItems.Select(cartItem => new OrderDetail
            {
                ItemId = cartItem.Item_Id,
                Quantity = cartItem.Quantity,
              
            }).ToList();
        }

     
        private Order MapOrderDtoToOrder(OrderDto orderDto, int userId)
        {
            var order = new Order
            {
                UserId = userId,
                AddressLine1 = orderDto.AddressLine1,
                City = orderDto.City,
                State = orderDto.State,
                PostalCode = orderDto.PostalCode,
                Country = orderDto.Country,
                Status = orderDto.Status ?? "Pending", 
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                {
                    ItemId = od.ItemId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()  
            };

            return order;
        }
        private List<OrderDetailDto> ConvertOrderDetailsToDto(List<OrderDetail> orderDetails)
        {
            return orderDetails.Select(od => new OrderDetailDto
            {
                ItemId = od.ItemId,
                Quantity = (int)od.Quantity,
                Price = od.Price
            }).ToList();
        }


    }

}
