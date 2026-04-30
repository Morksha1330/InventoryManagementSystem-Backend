using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<CategoryDTO>> GetPagedCategoriesAsync(RequestFilterDto filter)
        {
            var query = _context.Categories
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .AsQueryable();

            // 🔍 Search
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(c =>
                    c.CategoryName.Contains(filter.SearchTerm) ||
                    c.CategoryCode.Contains(filter.SearchTerm));
            }

            // 🔍 Active filter
            if (filter.Active.HasValue)
            {
                query = query.Where(c => c.Active == filter.Active.Value);
            }

            var totalCount = await query.CountAsync();

            // 🔽 Sorting
            query = filter.SortBy?.ToLower() switch
            {
                "categoryname" => filter.SortOrder == "DESC"
                    ? query.OrderByDescending(c => c.CategoryName)
                    : query.OrderBy(c => c.CategoryName),

                "categorycode" => filter.SortOrder == "DESC"
                    ? query.OrderByDescending(c => c.CategoryCode)
                    : query.OrderBy(c => c.CategoryCode),

                _ => filter.SortOrder == "DESC"
                    ? query.OrderByDescending(c => c.Id)
                    : query.OrderBy(c => c.Id)
            };

            // 📄 Pagination
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    CategoryCode = c.CategoryCode,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    Active = c.Active,
                    CreatedDate = c.CreatedDate,
                    ProductCount = c.Products.Count,
                    SubCategoryCount = c.SubCategories.Count
                })
                .ToListAsync();

            return new PagedResultDto<CategoryDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existing = await _context.Categories.FindAsync(category.Id);
            if (existing == null) return null;

            existing.CategoryCode = category.CategoryCode;
            existing.CategoryName = category.CategoryName;
            existing.Description = category.Description;
            existing.Active = category.Active;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return false;

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}