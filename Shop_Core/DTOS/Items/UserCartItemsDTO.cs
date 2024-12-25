using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Items
{
    public class UserCartItemsDTO
    {
        public string name { get; set; }
        public double price { get; set; }
        public string ItemUnit { get; set; }
        public double Quantity { get; set; }

    }
}
