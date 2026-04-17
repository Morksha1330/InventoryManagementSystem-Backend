using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IPurchaseOrderRepository
    {
        IEnumerable<PurchaseOrder> GetAll();
        PurchaseOrder GetData(int id);
        void Add(PurchaseOrder data);
        void Update(PurchaseOrder data);
        void Delete(int id);
        bool Save();
    }
}
