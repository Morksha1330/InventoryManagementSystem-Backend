using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface ISalesDetailsRepository
    {
        IEnumerable<SalesOrderItem> GetAll();
        SalesOrderItem GetData(int id);
        void Add(SalesOrderItem data);
        void Update(SalesOrderItem data);
        void Delete(int id);
        bool Save();
    }
}
