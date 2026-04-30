using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repositories.Interface;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace InventoryMgtSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return null;

            existing.ProductName = product.ProductName;
            existing.CategoryId = product.CategoryId;
            existing.SKU = product.SKU;
            existing.UnitPrice = product.UnitPrice;
            existing.TotalQuantity = product.TotalQuantity;
            existing.Active = product.Active;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<ProductDTO>> GetPagedProductsAsync(RequestFilterDto filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsQueryable();

            // 🔍 Search
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(p =>
                    p.ProductName.Contains(filter.SearchTerm) ||
                    p.SKU.Contains(filter.SearchTerm) ||
                    p.Category.CategoryName.Contains(filter.SearchTerm));
            }

            // 🔍 Active filter
            if (filter.Active.HasValue)
            {
                query = query.Where(p => p.Active == filter.Active.Value);
            }

            var totalCount = await query.CountAsync();

            // 🔽 Sorting
            query = filter.SortBy?.ToLower() switch
            {
                "productname" => filter.SortOrder == "DESC"
                    ? query.OrderByDescending(p => p.ProductName)
                    : query.OrderBy(p => p.ProductName),

                "unitprice" => filter.SortOrder == "DESC"
                    ? query.OrderByDescending(p => p.UnitPrice)
                    : query.OrderBy(p => p.UnitPrice),

                _ => filter.SortOrder == "DESC"
                    ? query.OrderByDescending(p => p.Id)
                    : query.OrderBy(p => p.Id)
            };

            // 📄 Pagination
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    SKU = p.SKU,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : "",
                    SubCategoryId = p.SubCategoryId,
                    SubCategoryName = p.SubCategory != null ? p.SubCategory.SubCategoryName:"",
                    TotalQuantity = p.TotalQuantity,
                    UnitPrice = p.UnitPrice,
                    Active = p.Active,
                    CreatedDate = p.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<ProductDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
    }
}