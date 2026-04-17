using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IFormsRepository
    {
        IEnumerable<Form> GetAll();
        Form GetData(int id);
        void Add(Form data);
        void Update(Form data);
        void Delete(int id);
        bool Save();
    }
}
