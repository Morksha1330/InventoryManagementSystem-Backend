using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Product data)
        {
            _context.Products.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.Products.Find(id);
            if (data != null)
            {
                _context.Products.Remove(data);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetData(int id)
        {
            var data = _context.Products.FirstOrDefault(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Product data)
        {
            _context.Products.Update(data);
        }
    }
}
