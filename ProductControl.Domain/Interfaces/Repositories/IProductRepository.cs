using ProductControl.Domain.Entities;

namespace ProductControl.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task AddAsync(Product product);
        public void UpdateAsync(Product product);
        public Task<Product?> GetByIdAsync(Guid id);
        public Task<bool> ValidateRegisteredName(string name);
    }
}
