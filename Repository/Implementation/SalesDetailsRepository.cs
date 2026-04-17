using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class SalesDetailsRepository : ISalesDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(SalesOrderItem data)
        {
            _context.SalesOrderItems.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.SalesOrderItems.Find(id);
            if (data != null)
            {
                _context.SalesOrderItems.Remove(data);
            }
        }

        public IEnumerable<SalesOrderItem> GetAll()
        {
            return _context.SalesOrderItems.ToList();
        }

        public SalesOrderItem GetData(int id)
        {
            var data = _context.SalesOrderItems.FirstOrDefault(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(SalesOrderItem data)
        {
            _context.SalesOrderItems.Update(data);
        }
    }
}
