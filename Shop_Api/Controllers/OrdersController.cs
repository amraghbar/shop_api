using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Api.HF;
using Shop_Core.DTOS.Order;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("addorderfromcart")]
        public async Task<IActionResult> AddOrderFromCart([FromBody] OrderDto placeOrderDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            var cartItems = await unitOfWork.CartRepository.GetAllItemsFromCart(userId.Value);
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

            var order = MapOrderDtoToOrder(placeOrderDto, userId.Value);
            order.OrderDetails = orderDetails;
            order.CalculateTotalAmount();

            var createdOrder = await unitOfWork.OrderRepository.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }



        [HttpGet("getwithid")]
        public async Task<IActionResult> GetOrderById([FromQuery] int orderId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(orderId, userId.Value);
            if (order == null)
            {
                return NotFound("Order not found or unauthorized.");
            }

            order.CalculateTotalAmount();
            return Ok(order);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetOrdersByUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            try
            {
                var orders = await unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId.Value);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromQuery] int orderId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            var isDeleted = await unitOfWork.OrderRepository.DeleteOrderAsync(orderId, userId.Value);
            if (!isDeleted)
            {
                return NotFound("Order not found or unauthorized.");
            }

            return NoContent();
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
    }
}
