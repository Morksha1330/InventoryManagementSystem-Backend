using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface ISalesRepository
    {
        IEnumerable<SalesOrder> GetAll();
        SalesOrder GetData(int id);
        void Add(SalesOrder data);
        void Update(SalesOrder data);
        void Delete(int id);
        bool Save();
    }
}
