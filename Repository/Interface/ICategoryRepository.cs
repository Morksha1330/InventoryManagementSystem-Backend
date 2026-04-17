using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category GetData(int id);
        void Add(Category data);
        void Update(Category data);
        void Delete(int id);
        bool Save();

    }
}
