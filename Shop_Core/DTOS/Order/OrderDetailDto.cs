using Shop_Core.DTOS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Order
{

    public class OrderDetailDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public ItemOrD Items { get; set; }


    }
}
