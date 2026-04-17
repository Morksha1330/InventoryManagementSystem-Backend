using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class CategoriesRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Category category)
        {
            _context.Add<Category>(category);
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetUser(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                throw new KeyNotFoundException("Category Not Fount");

            return category;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}

