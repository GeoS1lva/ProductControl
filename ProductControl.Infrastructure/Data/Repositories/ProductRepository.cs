using Microsoft.EntityFrameworkCore;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Infrastructure.Data.Context;

namespace ProductControl.Infrastructure.Data.Repositories
{
    public class ProductRepository(PostgreDbContext _context) : IProductRepository
    {
        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);

        public void UpdateAsync(Product product)
            => _context.Products.Update(product);

        public async Task<Product?> GetByIdAsync(Guid id)
            => await _context.Products.FindAsync(id);

        public async Task<bool> ValidateRegisteredName(string name)
            => await _context.Products.AnyAsync(u => u.Name == name);
    }
}
