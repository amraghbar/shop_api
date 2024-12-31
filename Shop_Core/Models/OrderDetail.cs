using Shop_Core.DTOS;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shop_Core.Models
{

    public class OrderDetail
    {
        public int Id { get; set; } 

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [ForeignKey(nameof(Items))]
        public int ItemId { get; set; } 

        public double Quantity { get; set; } 
        public decimal Price { get; set; } 
        public decimal Total => (decimal)Quantity * Price;
        public Order Order { get; set; }
        public Items Items { get; set; }
    }
}
