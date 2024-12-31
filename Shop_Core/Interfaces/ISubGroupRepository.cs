using Shop_Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface ISubGroupRepository
    {
        Task<IEnumerable<SubGroupDto>> GetAllAsync();
        Task<SubGroupDto> GetByIdAsync(int id);
        Task AddAsync(SubGroupDto subGroupDto);
        Task UpdateAsync(SubGroupDto subGroupDto);
        Task DeleteAsync(int id);
    }
}
