using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Order
{
    public class OrderDTOs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public void CalculateTotalAmount()
        {
            TotalAmount = OrderDetails.Sum(od => od.Total);
        }
    }

}
