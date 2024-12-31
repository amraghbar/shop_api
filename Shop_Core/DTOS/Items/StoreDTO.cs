using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Items
{
    public class StoreDTO
    {
        public int StoreId { get; set; } // المعرف الفريد للمخزن
        public string StoreName { get; set; } // اسم المخزن
        public double Balance { get; set; } // الكمية المتوفرة
        public DateTime LastUpdated { get; set; } // تاريخ آخر تحديث
    }
}
