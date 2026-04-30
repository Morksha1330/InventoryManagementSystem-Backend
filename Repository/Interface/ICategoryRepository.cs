using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<PagedResultDto<CategoryDTO>> GetPagedCategoriesAsync(RequestFilterDto filter);

        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}