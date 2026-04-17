using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetAll();
        Product GetData(int id);
        void Add(Product data);
        void Update(Product data);
        void Delete(int id);
        bool Save();
    }
}
