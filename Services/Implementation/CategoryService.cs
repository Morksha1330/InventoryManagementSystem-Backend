using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using InventoryMgtSystem.Services.Interface;

namespace InventoryMgtSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<HttpResponseData<PagedResultDto<CategoryDTO>>> GetPagedCategoriesAsync(RequestFilterDto filter)
        {
            var response = new HttpResponseData<PagedResultDto<CategoryDTO>>();

            try
            {
                filter.PageNumber = Math.Max(1, filter.PageNumber);
                filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);
                filter.SortBy = string.IsNullOrWhiteSpace(filter.SortBy) ? "Id" : filter.SortBy;
                filter.SortOrder = filter.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

                var result = await _repo.GetPagedCategoriesAsync(filter);

                response.Result = result;
                response.Success = true;
                response.ResponsCode = 200;
                response.Message = "Categories retrieved successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "Error retrieving categories";
            }

            return response;
        }

        public async Task<HttpResponseData<CategoryDTO>> GetAllCategories()
        {
            var response = new HttpResponseData<CategoryDTO>();

            var categories = await _repo.GetAllAsync();

            response.Results = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                CategoryCode = c.CategoryCode,
                CategoryName = c.CategoryName,
                Active = c.Active
            }).ToList();

            response.Success = true;
            return response;
        }

        public async Task<HttpResponseData<CategoryDTO>> GetCategoryById(int id)
        {
            var response = new HttpResponseData<CategoryDTO>();

            var category = await _repo.GetByIdAsync(id);

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found";
                return response;
            }

            response.Result = new CategoryDTO
            {
                Id = category.Id,
                CategoryCode = category.CategoryCode,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Active = category.Active
            };

            response.Success = true;
            return response;
        }

        public async Task<HttpResponseData<CategoryDTO>> AddCategory(CategoryDTO dto)
        {
            var response = new HttpResponseData<CategoryDTO>();

            var category = new Category
            {
                CategoryCode = dto.CategoryCode,
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                Active = dto.Active
            };

            var created = await _repo.AddAsync(category);

            response.Result = new CategoryDTO
            {
                Id = created.Id,
                CategoryName = created.CategoryName
            };

            response.Success = true;
            response.Message = "Category created successfully";

            return response;
        }

        public async Task<HttpResponseData<CategoryDTO>> UpdateCategory(CategoryDTO dto)
        {
            var response = new HttpResponseData<CategoryDTO>();

            var category = new Category
            {
                Id = dto.Id,
                CategoryCode = dto.CategoryCode,
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                Active = dto.Active
            };

            var updated = await _repo.UpdateAsync(category);

            if (updated == null)
            {
                response.Success = false;
                response.Message = "Category not found";
                return response;
            }

            response.Success = true;
            response.Message = "Category updated successfully";
            return response;
        }

        public async Task<HttpResponseData<CategoryDTO>> DeleteCategory(int id)
        {
            var response = new HttpResponseData<CategoryDTO>();

            var deleted = await _repo.DeleteAsync(id);

            if (!deleted)
            {
                response.Success = false;
                response.Message = "Category not found";
                return response;
            }

            response.Success = true;
            response.Message = "Category deleted successfully";
            return response;
        }
    }
}