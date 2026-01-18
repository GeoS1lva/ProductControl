using ProductControl.Domain.Entities;

namespace ProductControl.Domain.Interfaces.Repositories
{
    public interface IStockMovementRepository
    {
        public Task<List<StockMovement>?> GetByProductStockMovementsAsync(Guid productId);
        public Task<List<StockMovement>?> GetByUserStockMovementsAsync(Guid userId);
        public Task<List<StockMovement>?> GetAllStockMovementsAsync();
    }
}
