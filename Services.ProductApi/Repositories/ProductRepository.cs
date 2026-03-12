using Microsoft.EntityFrameworkCore;
using Services.ProductApi.Data;
using Services.ProductApi.Mapper;
using Services.ProductApi.Models.Dto;
using Services.ProductApi.Repositories.Interfaces;

namespace Services.ProductApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        // Project directly to DTOs to avoid loading entities unnecessarily
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(c => c.ToProductDto())
                .ToListAsync();
        }

        public async Task<ProductDto?> GetById(int productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(c => c.ProductId == productId)
                .Select(c => c.ToProductDto())
                .FirstOrDefaultAsync();
        }


        public async Task<ProductDto> Create(RequestDto product)
        {
            var newProduct = product.ToProductEntity();
            _context.Products.Add(newProduct);
            return await _context.SaveChangesAsync()
                .ContinueWith(t => newProduct.ToProductDto());
        }

        public async Task<bool?> Update(RequestDto product, int productId)
        {
            var existing = _context.Products.Find(productId);
            if (existing == null)
                return null;
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.CategoryName = product.CategoryName;
            existing.ImageUrl = product.ImageUrl;
            existing.ImageLocalPath = product.ImageLocalPath;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> DeleteById(int productId)
        {
            var existing = _context.Products.Find(productId);
            if (existing == null)
                return null;
            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
