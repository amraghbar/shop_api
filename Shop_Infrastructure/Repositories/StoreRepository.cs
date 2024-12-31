using Microsoft.EntityFrameworkCore;
using Shop_Core.DTOS.Store;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext _context;

        public StoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StoreReadDto>> GetAllStoresAsync()
        {
            return await _context.Stores
                .Select(store => new StoreReadDto
                {
                    Id = store.Id,
                    Name = store.Name,
                    GovName = store.Governments.Name,
                    CityName = store.Cities.Name
                })
                .ToListAsync();
        }

        public async Task<StoreReadDto> GetStoreByIdAsync(int id)
        {
            var store = await _context.Stores
                .Include(s => s.Governments)
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (store == null)
                return null;

            return new StoreReadDto
            {
                Id = store.Id,
                Name = store.Name,
                GovName = store.Governments.Name,
                CityName = store.Cities.Name
            };
        }
        public async Task<StoreReadDto> AddStoreAsync(StoreDto storeDto)
        {
            var govId = await _context.Governments
                                     .Where(g => g.Name == storeDto.GovName)
                                     .Select(g => g.Id)
                                     .FirstOrDefaultAsync();

            if (govId == 0)
                throw new Exception("Government not found.");

            var cityId = await _context.Cities
                                      .Where(c => c.Name == storeDto.CityName)
                                      .Select(c => c.Id)
                                      .FirstOrDefaultAsync();

            if (cityId == 0)
                throw new Exception("City not found.");

            var store = new Stores
            {
                Name = storeDto.Name,
                Gov_Id = govId,
                City_Id = cityId
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            ;

            return new StoreReadDto
            {
                Id = storeDto.Id = store.Id, 
                Name = store.Name,
                GovName = storeDto.GovName,
                CityName = storeDto.CityName
            };


        }


        public async Task<bool> UpdateStoreAsync(int id, StoreDto storeDto)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
                return false;

            store.Name = storeDto.Name;
            store.Gov_Id = await _context.Governments
                             .Where(g => g.Name == storeDto.GovName)
                             .Select(g => g.Id)
                             .FirstOrDefaultAsync();
            store.City_Id = await _context.Cities
                             .Where(c => c.Name == storeDto.CityName)
                             .Select(c => c.Id)
                             .FirstOrDefaultAsync();

            _context.Stores.Update(store);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
                return false;

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return true;
        }

        
    }

}
