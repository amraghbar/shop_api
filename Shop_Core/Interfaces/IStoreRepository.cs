using Shop_Core.DTOS.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreReadDto>> GetAllStoresAsync();
        Task<StoreReadDto> GetStoreByIdAsync(int id);
        Task<StoreReadDto> AddStoreAsync(StoreDto storeDto);
        Task<bool> UpdateStoreAsync(int id, StoreDto storeDto);
        Task<bool> DeleteStoreAsync(int id);
    }

}
