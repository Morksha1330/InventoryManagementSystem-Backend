using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class SalesOrderRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SalesOrder data)
        {
            _context.SalesOrders.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.SalesOrders.Find(id);
            if (data != null)
            {
                _context.SalesOrders.Remove(data);
            }
        }

        public IEnumerable<SalesOrder> GetAll()
        {
            return _context.SalesOrders.ToList();
        }

        public SalesOrder GetData(int id)
        {
            var data = _context.SalesOrders.FirstOrDefault(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(SalesOrder data)
        {
            _context.SalesOrders.Update(data);
        }
    }
}
