using InventoryMgtSystem.Data;
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
    }
}