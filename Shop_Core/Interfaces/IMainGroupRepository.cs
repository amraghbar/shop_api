using Shop_Core.DTOS;
using Shop_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IMainGroupRepository
    {
        Task<IEnumerable<MainGroupDto>> GetAllAsync(); // عرض الكل باستخدام DTO
        Task<MainGroupDto> GetByIdAsync(int id); // عرض عنصر محدد باستخدام DTO
        Task AddAsync(MainGroupDto mainGroupDto); // إضافة عنصر باستخدام DTO
        Task UpdateAsync(MainGroupDto mainGroupDto); // تعديل عنصر باستخدام DTO
        Task DeleteAsync(int id); // حذف عنصر
    }
}
