using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;

namespace InventoryMgtSystem.Services.Interface
{
    public interface ICategoryService
    {
        Task<HttpResponseData<PagedResultDto<CategoryDTO>>> GetPagedCategoriesAsync(RequestFilterDto filter);

        Task<HttpResponseData<CategoryDTO>> GetAllCategories();
        Task<HttpResponseData<CategoryDTO>> GetCategoryById(int id);
        Task<HttpResponseData<CategoryDTO>> AddCategory(CategoryDTO dto);
        Task<HttpResponseData<CategoryDTO>> UpdateCategory(CategoryDTO dto);
        Task<HttpResponseData<CategoryDTO>> DeleteCategory(int id);
    }
}