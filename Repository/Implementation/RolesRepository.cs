using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class RolesRepository : IRolesRepository
    {
        private readonly ApplicationDbContext _context;
        public RolesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Role data)
        {
            _context.Roles.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.Roles.Find(id);
            if (data != null)
            {
                _context.Roles.Remove(data);
            }
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }

        public async Task<Role> GetData(int id)
        {
            var data =  await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Role data)
        {
            _context.Roles.Update(data);
        }
    }
}
