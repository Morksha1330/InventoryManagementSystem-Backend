using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IStockAlertsRepository
    {
        IEnumerable<Stock> GetAll();
        Stock GetData(int id);
        void Add(Stock data);
        void Update(Stock data);
        void Delete(int id);
        bool Save();
    }
}

