using Shop_Core.DTOS;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shop_Core.Models
{

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; } // FK to Users
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        // Shipping Address
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Shipped, Delivered

        // Navigation Properties
        public ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();


        public void CalculateTotalAmount()
        {
            TotalAmount = OrderDetails.Sum(od => od.Total);
        }
    }
}
