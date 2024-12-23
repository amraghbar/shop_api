using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Items
{
    public class PagedResponse<ItemsDto>
    {
        public int total_items { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
        public IEnumerable<ItemsDTO> items { get; set; }
    }
}
