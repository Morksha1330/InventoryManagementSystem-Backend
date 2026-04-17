using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Supplier supplier)
        {
           _context.Add<Supplier>(supplier);
        }

        public void Delete(int id)
        {
            var user = _context.Suppliers.Find(id);
            if (user != null)
            {
                _context.Suppliers.Remove(user);
            }
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _context.Suppliers.ToList();
        }

        public Supplier GetUser(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == id);
            if (supplier == null)
                throw new KeyNotFoundException("User Not Fount");

            return supplier;
        } 

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Supplier user)
        {
            _context.Suppliers.Update(user);
        }
    }
}
