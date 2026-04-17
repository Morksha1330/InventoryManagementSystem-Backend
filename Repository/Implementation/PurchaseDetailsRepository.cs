using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class PurchaseDetailsRepository : IPurchaseDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(PurchaseOrderItem data)
        {
            _context.PurchaseOrderItem.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.PurchaseOrderItem.Find(id);
            if (data != null)
            {
                _context.PurchaseOrderItem.Remove(data);
            }
        }

        public IEnumerable<PurchaseOrderItem> GetAll()
        {
            return _context.PurchaseOrderItem.ToList();
        }

        public PurchaseOrderItem GetData(int id)
        {
            var data = _context.PurchaseOrderItem.FirstOrDefault(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(PurchaseOrderItem data)
        {
            _context.PurchaseOrderItem.Update(data);
        }
    }
}
