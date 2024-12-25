using Microsoft.EntityFrameworkCore;
using Shop_Core.DTOS.Items;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Infrastructure.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly AppDbContext appDbContext;

        public ItemsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        // جلب العناصر مع التفاصيل
        public async Task<IEnumerable<ItemsDTO>> GetItemsAsync()
        {
            var items = await appDbContext.Items
                .Include(x => x.ItemsUnits)
                .Include(x => x.InvItemStores)
                .Select(x => new ItemsDTO
                {
                      Name = x.Name,
                    price = x.price,
                    ItemUnits = x.ItemsUnits.Select(unit => unit.Units.Name).ToList(),
                    Stores = x.InvItemStores.Select(store => store.Stores.Name).ToList(),
                })
                .ToListAsync();
            return items;
        }

        // تقسيم العناصر إلى صفحات
        public async Task<PagedResponse<ItemsDTO>> PaginationAsync(IQueryable<ItemsDTO> query, int page_index, int page_size)
        {
            var total_items = await query.CountAsync();

            var items = await query
                .Skip((page_index - 1) * page_size)
                .Take(page_size)
                .ToListAsync();

            return new PagedResponse<ItemsDTO>
            {
                total_items = total_items,
                items = items,
                page_index = page_index,
                page_size = page_size
            };
        }

        // إضافة عنصر جديد
        public async Task<ItemsDTO> AddItemAsync(Items newItem)
        {
            if (newItem == null)
                throw new ArgumentNullException(nameof(newItem));

            foreach (var unit in newItem.ItemsUnits)
            {
                if (!await appDbContext.Units.AnyAsync(u => u.Name == unit.Units.Name))
                {
                    throw new InvalidOperationException($"Unit {unit.Units.Name} does not exist.");
                }
            }

            foreach (var store in newItem.InvItemStores)
            {
                if (!await appDbContext.Stores.AnyAsync(s => s.Name == store.Stores.Name))
                {
                    throw new InvalidOperationException($"Store {store.Stores.Name} does not exist.");
                }
            }

            appDbContext.Items.Add(newItem);
            await appDbContext.SaveChangesAsync();

            return new ItemsDTO
            {
                Name = newItem.Name,
                price = newItem.price,
                ItemUnits = newItem.ItemsUnits.Select(unit => unit.Units.Name).ToList(),
                Stores = newItem.InvItemStores.Select(store => store.Stores.Name).ToList(),

            };
        }

        // تعديل عنصر موجود
        public async Task<ItemsDTO> UpdateItemAsync(int id, Items updatedItem)
        {
            if (updatedItem == null)
                throw new ArgumentNullException(nameof(updatedItem));

            var existingItem = await appDbContext.Items
                .Include(x => x.ItemsUnits)
                .Include(x => x.InvItemStores)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingItem == null)
                throw new KeyNotFoundException($"Item with ID {id} not found.");

            // تحديث البيانات
            existingItem.Name = updatedItem.Name;
            existingItem.Description = updatedItem.Description;
            existingItem.price = updatedItem.price;
            existingItem.MG_Id = updatedItem.MG_Id;
            existingItem.Sub_Id = updatedItem.Sub_Id;

            // تحديث وحدات العناصر والمخازن إذا لزم الأمر
            existingItem.ItemsUnits = updatedItem.ItemsUnits;
            existingItem.InvItemStores = updatedItem.InvItemStores;

            appDbContext.Items.Update(existingItem);
            await appDbContext.SaveChangesAsync();

            return new ItemsDTO
            {
                Name = existingItem.Name,
                price = existingItem.price,
                ItemUnits = existingItem.ItemsUnits.Select(unit => unit.Units.Name).ToList(),
                Stores = existingItem.InvItemStores.Select(store => store.Stores.Name).ToList(),
            };
        }

        // حذف عنصر
        public async Task<bool> DeleteItemAsync(int id)
        {
            var existingItem = await appDbContext.Items.FindAsync(id);
            if (existingItem == null)
                throw new KeyNotFoundException($"Item with ID {id} not found.");

            appDbContext.Items.Remove(existingItem);
            await appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
