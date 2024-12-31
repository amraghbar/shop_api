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

        public async Task<int> GetAvailableQuantityAsync(int itemId, int storeId)
        {
            var invItemStore = await appDbContext.InvItemStores
                .FirstOrDefaultAsync(e => e.Item_Id == itemId && e.Store_Id == storeId);

            return (int)(invItemStore?.Balance ?? 0); // إرجاع الكمية أو 0 إذا لم يتم العثور على العنصر
        }


        // جلب العناصر مع التفاصيل
        public async Task<IEnumerable<ItemsDTO>> GetItemsAsync()
        {
            var items = await appDbContext.Items
                .AsNoTracking()  // لتحسين الأداء
                .Include(x => x.ItemsUnits)
                    .ThenInclude(x => x.Units)
                .Include(x => x.InvItemStores)
                    .ThenInclude(x => x.Stores)
                .Select(x => new ItemsDTO
                {
                    Id = x.Id,  // تأكد من إضافة id هنا
                    Name = x.Name,
                    price = x.price,
                    Description = x.Description,
                    MG_Id = x.MG_Id,
                    Sub_Id = x.Sub_Id,
                    ItemUnits = x.ItemsUnits.Select(unit => unit.Units.Name).ToList(),
                    Stores = x.InvItemStores.Select(store => new StoreDTO
                    {
                        StoreName = store.Stores.Name,
                        Balance = store.Balance,
                        LastUpdated = store.LastUpdated
                    }).ToList(),
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

            // تحقق من وجود الوحدات وربطها
            foreach (var unit in newItem.ItemsUnits)
            {
                var existingUnit = await appDbContext.Units.FirstOrDefaultAsync(u => u.Name == unit.Units.Name);
                if (existingUnit == null)
                    throw new InvalidOperationException($"Unit {unit.Units.Name} does not exist.");
                unit.Units = existingUnit;
            }

            // تحقق من وجود المتاجر وربطها
            foreach (var store in newItem.InvItemStores)
            {
                var existingStore = await appDbContext.Stores.FirstOrDefaultAsync(s => s.Name == store.Stores.Name);
                if (existingStore == null)
                    throw new InvalidOperationException($"Store {store.Stores.Name} does not exist.");

                store.Stores = existingStore;
                store.LastUpdated = DateTime.UtcNow;
            }

            // إضافة العنصر الجديد
            appDbContext.Items.Add(newItem);
            await appDbContext.SaveChangesAsync();

            return await BuildItemDTO(newItem.Id);
        }

        // تعديل عنصر موجود
        public async Task<ItemsDTO> UpdateItemAsync(int id, Items updatedItem)
        {
            if (updatedItem == null)
                throw new ArgumentNullException(nameof(updatedItem));

            var existingItem = await appDbContext.Items
                .Include(x => x.ItemsUnits)
                .ThenInclude(x => x.Units)
                .Include(x => x.InvItemStores)
                .ThenInclude(x => x.Stores)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingItem == null)
                throw new KeyNotFoundException($"Item with ID {id} not found.");

            // Update main fields
            existingItem.Name = updatedItem.Name;
            existingItem.Description = updatedItem.Description;
            existingItem.price = updatedItem.price;
            existingItem.MG_Id = updatedItem.MG_Id;
            existingItem.Sub_Id = updatedItem.Sub_Id;

            // Update units
            existingItem.ItemsUnits.Clear();
            foreach (var unit in updatedItem.ItemsUnits)
            {
                var existingUnit = await appDbContext.Units.FirstOrDefaultAsync(u => u.Name == unit.Units.Name);
                if (existingUnit == null)
                    throw new InvalidOperationException($"Unit {unit.Units.Name} does not exist.");

                existingItem.ItemsUnits.Add(new ItemsUnits { Units = existingUnit });
            }

            // Update stores (Now we update instead of clearing and adding)
            foreach (var store in updatedItem.InvItemStores)
            {
                var existingStore = await appDbContext.Stores.FirstOrDefaultAsync(s => s.Name == store.Stores.Name);
                if (existingStore == null)
                    throw new InvalidOperationException($"Store {store.Stores.Name} does not exist.");

                var invItemStore = existingItem.InvItemStores
                    .FirstOrDefault(x => x.Stores.Id == existingStore.Id);

                if (invItemStore != null)
                {
                    // Update the existing store's balance and last updated
                    invItemStore.Balance = store.Balance;
                    invItemStore.LastUpdated = DateTime.UtcNow;
                }
                else
                {
                    // If this store is not found in the existing stores, we add a new entry
                    existingItem.InvItemStores.Add(new InvItemStores
                    {
                        Stores = existingStore,
                        Balance = store.Balance,  // Ensure Balance is updated correctly
                        LastUpdated = DateTime.UtcNow
                    });
                }
            }

            // Update the item in the database
            appDbContext.Items.Update(existingItem);
            await appDbContext.SaveChangesAsync();

            return await BuildItemDTO(existingItem.Id);
        }


        // حذف عنصر
        public async Task<bool> DeleteItemAsync(int id)
        {
            var existingItem = await appDbContext.Items
                .Include(x => x.InvItemStores)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingItem == null)
                throw new KeyNotFoundException($"Item with ID {id} not found.");

            // إزالة الروابط ذات الصلة
            appDbContext.InvItemStores.RemoveRange(existingItem.InvItemStores);
            appDbContext.Items.Remove(existingItem);
            await appDbContext.SaveChangesAsync();

            return true;
        }

        // بناء كائن ItemsDTO
        private async Task<ItemsDTO> BuildItemDTO(int id)
        {
            var item = await appDbContext.Items
                .Include(x => x.ItemsUnits)
                .ThenInclude(x => x.Units)
                .Include(x => x.InvItemStores)
                .ThenInclude(x => x.Stores)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                throw new KeyNotFoundException($"Item with ID {id} not found.");

            return new ItemsDTO
            {
                Id = item.Id,
                Name = item.Name,
                price = item.price,
                Description = item.Description,
                MG_Id = item.MG_Id,
                Sub_Id = item.Sub_Id,
                ItemUnits = item.ItemsUnits.Select(unit => unit.Units.Name).ToList(),
                Stores = item.InvItemStores.Select(store => new StoreDTO
                {
                    StoreId = store.Stores.Id,
                    StoreName = store.Stores.Name,
                    Balance = store.Balance,
                    LastUpdated = store.LastUpdated
                }).ToList(),
            };
        }
    }
}
