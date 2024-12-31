using Microsoft.EntityFrameworkCore;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Infrastructure.Repositories
{
    public class SubGroupRepository : ISubGroupRepository
    {
        private readonly AppDbContext _context;

        public SubGroupRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SubGroupDto>> GetAllAsync()
        {
            return await _context.SubGroup.Include(sg => sg.MainGroup)
                .Select(sg => new SubGroupDto
                {
                    Id = sg.Id,
                    Name = sg.Name,
                    MainGroupName = sg.MainGroup.Name
                })
                .ToListAsync();
        }

        public async Task<SubGroupDto> GetByIdAsync(int id)
        {
            var subGroup = await _context.SubGroup
                .Include(sg => sg.MainGroup)
                .FirstOrDefaultAsync(sg => sg.Id == id);

            if (subGroup == null) return null;

            return new SubGroupDto
            {
                Id = subGroup.Id,
                Name = subGroup.Name,
                MainGroupName = subGroup.MainGroup.Name
            };
        }

        public async Task AddAsync(SubGroupDto subGroupDto)
        {
            var mainGroup = await _context.MainGroup.FirstOrDefaultAsync(mg => mg.Name == subGroupDto.MainGroupName);
            if (mainGroup == null) throw new KeyNotFoundException("MainGroup not found");

            var subGroup = new SubGroup
            {
                Name = subGroupDto.Name,
                MG_Id = mainGroup.Id
            };

            await _context.SubGroup.AddAsync(subGroup);
            await _context.SaveChangesAsync();

            subGroupDto.Id = subGroup.Id;
        }

        public async Task UpdateAsync(SubGroupDto subGroupDto)
        {
            var subGroup = await _context.SubGroup.FindAsync(subGroupDto.Id);
            if (subGroup == null) throw new KeyNotFoundException("SubGroup not found");

            var mainGroup = await _context.MainGroup.FirstOrDefaultAsync(mg => mg.Name == subGroupDto.MainGroupName);
            if (mainGroup == null) throw new KeyNotFoundException("MainGroup not found");

            subGroup.Name = subGroupDto.Name;
            subGroup.MG_Id = mainGroup.Id;

            _context.SubGroup.Update(subGroup);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var subGroup = await _context.SubGroup.FindAsync(id);
            if (subGroup != null)
            {
                _context.SubGroup.Remove(subGroup);
                await _context.SaveChangesAsync();
            }
        }
    }
}
