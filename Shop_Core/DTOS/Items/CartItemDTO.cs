using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Items
{
    public class CartItemDTO
    {
        public int ItemCode { get; set; }
        public int UnitCode { get; set; }
        public double Quantity { get; set; }
        public int storeId { get; set; }

    }
}
