using Microsoft.EntityFrameworkCore;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Infrastructure.Data.Context;

namespace ProductControl.Infrastructure.Data.Repositories
{
    public class StockMovementRepository(PostgreDbContext _context) : IStockMovementRepository
    {
        public async Task<List<StockMovement>?> GetByProductStockMovementsAsync(Guid productId)
            => await _context.StockMovements
                .Where(sm => sm.ProductId == productId)
                .ToListAsync();

        public async Task<List<StockMovement>?> GetByUserStockMovementsAsync(Guid userId)
            => await _context.StockMovements
                .Where(sm => sm.UserId == userId)
                .ToListAsync();

        public async Task<List<StockMovement>?> GetAllStockMovementsAsync()
            => await _context.StockMovements.ToListAsync();
    }
}
