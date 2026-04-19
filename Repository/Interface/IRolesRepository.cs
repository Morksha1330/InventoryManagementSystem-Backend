using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IRolesRepository
    {
        IEnumerable<Role> GetAll();
        Task<Role> GetData(int id);
        void Add(Role data);
        void Update(Role data);
        void Delete(int id);
        bool Save();
    }
}
