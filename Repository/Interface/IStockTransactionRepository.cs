using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IStockTransactionRepository
    {
        IEnumerable<StockTransaction> GetAll();
        StockTransaction GetData(int id);
        void Add(StockTransaction data);
        void Update(StockTransaction data);
        void Delete(int id);
        bool Save();
    }
}
