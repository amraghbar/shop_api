using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Items
{
    public class ItemsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double price { get; set; }
        public string Description { get; set; }
        public int MG_Id { get; set; }
        public int Sub_Id { get; set; }
        public List<string> ItemUnits { get; set; }

        // تعديل لتضمين تفاصيل المخازن
        public List<StoreDTO> Stores { get; set; }
    }
}

