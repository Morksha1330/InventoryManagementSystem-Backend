using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetUser(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        bool Save();
    }
}
