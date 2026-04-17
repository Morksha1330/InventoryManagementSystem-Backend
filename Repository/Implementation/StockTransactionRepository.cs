using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class StockTransactionRepository : IStockTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public StockTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(StockTransaction data)
        {
            _context.StockTransaction.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.StockTransaction.Find(id);
            if (data != null)
            {
                _context.StockTransaction.Remove(data);
            }
        }

        public IEnumerable<StockTransaction> GetAll()
        {
            return _context.StockTransaction.ToList();
        }

        public StockTransaction GetData(int id)
        {
            var data = _context.StockTransaction.FirstOrDefault(x => x.Id == id);
            if(data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(StockTransaction data)
        {
            _context.StockTransaction.Update(data);
        }
    }
}
