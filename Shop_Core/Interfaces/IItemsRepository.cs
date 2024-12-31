using Shop_Core.DTOS.Items;
using Shop_Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IItemsRepository
    {
        Task<IEnumerable<ItemsDTO>> GetItemsAsync();
        Task<PagedResponse<ItemsDTO>> PaginationAsync(IQueryable<ItemsDTO> query, int page_index, int page_size);
        Task<ItemsDTO> AddItemAsync(Items newItem);
        Task<ItemsDTO> UpdateItemAsync(int id, Items updatedItem);
        Task<bool> DeleteItemAsync(int id);
        Task<int> GetAvailableQuantityAsync(int itemId, int storeId);

    }
}
