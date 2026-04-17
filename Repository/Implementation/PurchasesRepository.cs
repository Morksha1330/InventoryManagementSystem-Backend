using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class PurchasesRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchasesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(PurchaseOrder data)
        {
            _context.PurchaseOrders.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.PurchaseOrders.Find(id);
            if (data != null)
            {
                _context.PurchaseOrders.Remove(data);
            }
        }

        public IEnumerable<PurchaseOrder> GetAll()
        {
            return _context.PurchaseOrders.ToList();
        }

        public PurchaseOrder GetData(int id)
        {
            var data = _context.PurchaseOrders.FirstOrDefault(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(PurchaseOrder data)
        {
            _context.PurchaseOrders.Update(data);
        }
    }
}
