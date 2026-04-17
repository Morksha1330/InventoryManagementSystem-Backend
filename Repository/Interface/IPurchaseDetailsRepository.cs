using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IPurchaseDetailsRepository
    {
        IEnumerable<PurchaseOrderItem> GetAll();
        PurchaseOrderItem GetData(int id);
        void Add(PurchaseOrderItem data);
        void Update(PurchaseOrderItem data);
        void Delete(int id);
        bool Save();
    }
}
