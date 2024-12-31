using Microsoft.EntityFrameworkCore;
using Shop_Core.DTOS;
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
    public class MainGroupRepository : IMainGroupRepository
    {
        private readonly AppDbContext appDbContext;

        public MainGroupRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<MainGroupDto>> GetAllAsync()
        {
            return await appDbContext.MainGroup
                .Select(mg => new MainGroupDto
                {
                    Id = mg.Id,
                    Name = mg.Name
                })
                .ToListAsync();
        }

        public async Task<MainGroupDto> GetByIdAsync(int id)
        {
            var mainGroup = await appDbContext.MainGroup.FindAsync(id);
            if (mainGroup == null) return null;

            return new MainGroupDto
            {
                Id = mainGroup.Id,
                Name = mainGroup.Name
            };
        }

        public async Task AddAsync(MainGroupDto mainGroupDto)
        {
            var mainGroup = new MainGroup
            {
                Name = mainGroupDto.Name
            };

            await appDbContext.MainGroup.AddAsync(mainGroup);
            await appDbContext.SaveChangesAsync();

            // يمكن تحديث الـ DTO بعد الحفظ إذا كان هناك حاجة (مثلاً لإضافة الـ Id)
            mainGroupDto.Id = mainGroup.Id;
        }

        public async Task UpdateAsync(MainGroupDto mainGroupDto)
        {
            var mainGroup = await appDbContext.MainGroup.FindAsync(mainGroupDto.Id);
            if (mainGroup == null) return;

            mainGroup.Name = mainGroupDto.Name;

            appDbContext.MainGroup.Update(mainGroup);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mainGroup = await appDbContext.MainGroup.FindAsync(id);
            if (mainGroup != null)
            {
                appDbContext.MainGroup.Remove(mainGroup);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}
