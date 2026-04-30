using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repositories;
using InventoryMgtSystem.Repositories.Interface;
using InventoryMgtSystem.Repository.Implementation;
using InventoryMgtSystem.Repository.Interface;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<HttpResponseData<PagedResultDto<ProductDTO>>> GetPagedProductsAsync(RequestFilterDto filter)
        {
            var response = new HttpResponseData<PagedResultDto<ProductDTO>>();

            try
            {
                // ✅ Sanitize inputs
                filter.PageNumber = Math.Max(1, filter.PageNumber);
                filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);
                filter.SortBy = string.IsNullOrWhiteSpace(filter.SortBy) ? "Id" : filter.SortBy;
                filter.SortOrder = filter.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

                var result = await _repo.GetPagedProductsAsync(filter);

                response.Result = result;
                response.Success = true;
                response.ResponsCode = 200;
                response.Message = "Products retrieved successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "Error retrieving products";
            }

            return response;
        }

        public async Task<HttpResponseData<ProductDTO>> GetAllProducts()
        {
            var response = new HttpResponseData<ProductDTO>();

            var products = await _repo.GetAllAsync();

            response.Results = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                SKU = p.SKU,
                UnitPrice = p.UnitPrice,
                Active = p.Active
            }).ToList();

            response.Success = true;
            return response;
        }

        public async Task<HttpResponseData<ProductDTO>> GetProductById(int id)
        {
            var response = new HttpResponseData<ProductDTO>();

            var product = await _repo.GetByIdAsync(id);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                return response;
            }

            response.Result = new ProductDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                SKU = product.SKU,
                UnitPrice = product.UnitPrice,
                Active = product.Active
            };

            response.Success = true;
            return response;
        }

        public async Task<HttpResponseData<ProductDTO>> AddProduct(ProductDTO dto)
        {
            var response = new HttpResponseData<ProductDTO>();

            var product = new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                SKU = dto.SKU,
                UnitPrice = dto.UnitPrice,
                Active = dto.Active
            };

            var created = await _repo.AddAsync(product);

            response.Result = new ProductDTO
            {
                Id = created.Id,
                ProductName = created.ProductName
            };

            response.Success = true;
            response.Message = "Product created successfully";

            return response;
        }

        public async Task<HttpResponseData<ProductDTO>> UpdateProduct(ProductDTO dto)
        {
            var response = new HttpResponseData<ProductDTO>();

            var product = new Product
            {
                Id = dto.Id,
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                SKU = dto.SKU,
                UnitPrice = dto.UnitPrice,
                Active = dto.Active
            };

            var updated = await _repo.UpdateAsync(product);

            if (updated == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                return response;
            }

            response.Success = true;
            response.Message = "Product updated successfully";
            return response;
        }

        public async Task<HttpResponseData<ProductDTO>> DeleteProduct(int id)
        {
            var response = new HttpResponseData<ProductDTO>();

            var deleted = await _repo.DeleteAsync(id);

            if (!deleted)
            {
                response.Success = false;
                response.Message = "Product not found";
                return response;
            }

            response.Success = true;
            response.Message = "Product deleted successfully";
            return response;
        }
    }
}