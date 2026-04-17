using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class StockAlertsRepository : IStockAlertsRepository
    {
        private readonly ApplicationDbContext _context;

        public StockAlertsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Stock data)
        {
            _context.Stock.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.Stock.Find(id);
            if(data != null)
            {
                _context.Stock.Remove(data);
            }
        }

        public IEnumerable<Stock> GetAll()
        {
            return _context.Stock.ToList();
        }

        public Stock GetData(int id)
        {
            var data = _context.Stock.FirstOrDefault(x => x.Id == id);
            if(data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Stock data)
        {
            _context.Stock.Update(data);
        }
    }
}
