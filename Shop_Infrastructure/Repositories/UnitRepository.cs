using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop_Infrastructure.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext appDbContext;

        public UnitRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IEnumerable<UD> GetAll()
        {
            return appDbContext.Units
                .Select(u => new UD { Id = u.Id, Name = u.Name })
                .ToList();
        }

        public UD GetById(int id)
        {
            var unit = appDbContext.Units
                .Where(u => u.Id == id)
                .Select(u => new UD { Id = u.Id, Name = u.Name })
                .FirstOrDefault();

            return unit;
        }

        public void Add(UD unit)
        {
            var newUnit = new Units
            {
                Name = unit.Name
            };
            appDbContext.Units.Add(newUnit);
            appDbContext.SaveChanges();
            unit.Id = newUnit.Id;

        }

        public void Update(UD unit)
        {
            var existingUnit = appDbContext.Units.FirstOrDefault(u => u.Id == unit.Id);

            if (existingUnit != null)
            {
                existingUnit.Name = unit.Name;

                // حفظ التغييرات
                appDbContext.SaveChanges();
            }
            else
            {
                // إذا لم يتم العثور على السجل
                throw new Exception($"Unit with ID {unit.Id} not found.");
            }
        }


        public void Delete(int id)
        {
            var unit = appDbContext.Units.FirstOrDefault(u => u.Id == id);
            if (unit != null)
            {
                appDbContext.Units.Remove(unit);
                appDbContext.SaveChanges();
            }
        }
    }
}
