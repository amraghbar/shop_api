using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_Api.HF;
using Shop_Core.DTOS.Cart;
using Shop_Core.Interfaces;

namespace Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CartsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("add_to_cart")]
        public async Task<ActionResult> AddBulkItemsToCart([FromBody] CartItemDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token");
                }

                // تحقق من توفر الكمية المطلوبة للعنصر المحدد
                var availableQuantity = await unitOfWork.ItemsRepository.GetAvailableQuantityAsync(dto.ItemCode,dto.storeId);
                if (availableQuantity < dto.Quantity)
                {
                    return BadRequest($"Only {availableQuantity} items are available for Item ID: {dto.ItemCode}");
                }

                var result = await unitOfWork.CartRepository.AddBulkQuantityToCartAsync(dto, userId);
                if (result == "Item added to cart successfully")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Unauthorized("Invalid Token: " + ex.Message);
            }
        }



        [HttpPost("add/one_to_cart")]
        public async Task<IActionResult> AddOneItemToCart([FromBody] CartItemDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("invalid user token");
                }
                var result = await unitOfWork.CartRepository.AddOneQuantityToCartAsync(dto, userId);
                if (result == "Item added to cart successfully")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Unauthorized("invalid Token: " + ex.Message);
            }
        }

        [HttpGet("get/cart/allitems")]
        public async Task<IActionResult> GetAllItemsFromCart()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("invalid user token");
                }

                var result = await unitOfWork.CartRepository.GetAllItemsFromCart(userId);

                if (result == null)
                {
                    return NotFound("No Items in your cart");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error " + ex.Message);
            }
        }

        [HttpDelete("clear_cart")]
        public async Task<IActionResult> ClearCart()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token");
                }

                var result = await unitOfWork.CartRepository.ClearCartAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error: " + ex.Message);
            }
        }

        [HttpDelete("remove_item")]
        public async Task<IActionResult> RemoveItemFromCart([FromQuery]int itemId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token");
                }

                var result = await unitOfWork.CartRepository.RemoveItemFromCartAsync(itemId, userId);
                if (result == "Item removed from cart successfully")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error: " + ex.Message);
            }
        }

        [HttpPut("update_quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromQuery] int itemId, [FromQuery] int quantity)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token");
                }

                var result = await unitOfWork.CartRepository.UpdateCartItemQuantityAsync(itemId, quantity, userId);
                if (result == "Cart item quantity updated successfully")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error: " + ex.Message);
            }
        }

        [HttpGet("get_cart_total")]
        public async Task<IActionResult> GetCartTotal()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token");
                }

                var total = await unitOfWork.CartRepository.GetCartTotalAsync(userId);
                return Ok(new { Total = total });
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error: " + ex.Message);
            }
        }

        [HttpGet("is_cart_empty")]
        public async Task<IActionResult> IsCartEmpty()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }

            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token");
                }

                var isEmpty = await unitOfWork.CartRepository.IsCartEmptyAsync(userId);
                return Ok(new { IsEmpty = isEmpty });
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error: " + ex.Message);
            }
        }
    }
}
