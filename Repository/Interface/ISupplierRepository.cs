using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetAll();
        Supplier GetUser(int id);
        void Add(Supplier user);
        void Update(Supplier user);
        void Delete(int id);
        bool Save();
    }
}
