using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class FormsRepository : IFormsRepository
    {
        private readonly ApplicationDbContext _context;

        public FormsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Form data)
        {
            _context.Forms.Add(data);
        }

        public void Delete(int id)
        {
            var data = _context.Forms.Find(id);
            if (data != null)
            {
                _context.Forms.Remove(data);
            }
        }

        public IEnumerable<Form> GetAll()
        {
            return _context.Forms.ToList();
        }

        public Form GetData(int id)
        {
            var data = _context.Forms.FirstOrDefault(x => x.Id == id);
            if (data == null)
                throw new KeyNotFoundException("Data Not Fount");
            return data;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Form data)
        {
            _context.Forms.Update(data);
        }
    }
}
