using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;

namespace InventoryMgtSystem.Services.Interface
{
    public interface IProductService
    {
        Task<HttpResponseData<PagedResultDto<ProductDTO>>> GetPagedProductsAsync(RequestFilterDto filter);
        Task<HttpResponseData<ProductDTO>> GetAllProducts();
        Task<HttpResponseData<ProductDTO>> GetProductById(int id);
        Task<HttpResponseData<ProductDTO>> AddProduct(ProductDTO dto);
        Task<HttpResponseData<ProductDTO>> UpdateProduct(ProductDTO dto);
        Task<HttpResponseData<ProductDTO>> DeleteProduct(int id);
    }
}