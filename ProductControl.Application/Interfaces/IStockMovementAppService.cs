using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.Interfaces
{
    public interface IStockMovementAppService
    {
        public Task<ResultPattern<List<StockMovement>?>> GetByProductStockMovementsAsync(Guid productId);
        public Task<ResultPattern<IList<StockMovement>?>> GetByUserStockMovementsAsync(Guid userId);
        public Task<ResultPattern<IList<StockMovement>?>> GetAllStockMovementsAsync();
    }
}
