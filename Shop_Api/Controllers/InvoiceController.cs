using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public InvoiceController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // إنشاء الفاتورة بناءً على الطلب
        [HttpPost("create-from-order")]
        public async Task<IActionResult> CreateInvoiceFromOrderAsync([FromQuery] int orderId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var result = await unitOfWork.InvoiceRepository.CreateInvoiceFromOrderAsync(orderId);

            if (result.Contains("Invoice created successfully"))
            {
                return Ok(new { message = result });
            }

            return BadRequest(new { message = result });
        }

        // استرجاع تفاصيل الفاتورة
        [HttpGet]
        public async Task<IActionResult> GetInvoiceReceipt([FromQuery]int customerId, [FromQuery] int invoiceId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var receipt = await unitOfWork.InvoiceRepository.GetInvoiceReciept(customerId, invoiceId);

            if (receipt == null)
            {
                return NotFound(new { message = "Invoice not found" });
            }

            return Ok(receipt);
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateInvoiceStatus([FromQuery]int invoiceId,
       [FromQuery] bool? isPosted,
       [FromQuery] bool? isReviewed,
       [FromQuery] bool? isClosed)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var result = await unitOfWork.InvoiceRepository.UpdateInvoiceStatusAsync(invoiceId, isPosted, isReviewed, isClosed);

            if (result.Contains("not found"))
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("customerinvoices")]
        public async Task<IActionResult> GetInvoicesByCustomerId([FromQuery] int customerId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var invoices = await unitOfWork.InvoiceRepository.GetInvoicesByCustomerIdAsync(customerId);

            if (invoices == null || !invoices.Any())
            {
                return NotFound("No invoices found for this customer.");
            }

            return Ok(invoices);
        }
    }


  
}
