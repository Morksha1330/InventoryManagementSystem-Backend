using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<PagedResultDto<ProductDTO>> GetPagedProductsAsync(RequestFilterDto filter);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}