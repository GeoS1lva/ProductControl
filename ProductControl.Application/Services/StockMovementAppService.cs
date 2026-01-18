using ProductControl.Application.Interfaces;
using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.Services
{
    public class StockMovementAppService(IUnitOfWork unitOfWork) : IStockMovementAppService
    {
        public async Task<ResultPattern<List<StockMovement>?>> GetByProductStockMovementsAsync(Guid productId)
        {
            var result = await unitOfWork.StockMovementRepository.GetByProductStockMovementsAsync(productId);

            if (result is null)
                return ResultPattern<List<StockMovement>?>.Failure("Não foi encontrado movimentaçoes.");

            return ResultPattern<List<StockMovement>?>.Success(result);
        }

        public async Task<ResultPattern<IList<StockMovement>?>> GetByUserStockMovementsAsync(Guid userId)
        {
            var result = await unitOfWork.StockMovementRepository.GetByUserStockMovementsAsync(userId);

            if (result is null)
                return ResultPattern<IList<StockMovement>?>.Failure("Não foi encontrado movimentaçoes.");

            return ResultPattern<IList<StockMovement>?>.Success(result);
        }

        public async Task<ResultPattern<IList<StockMovement>?>> GetAllStockMovementsAsync()
        {
            var result = await unitOfWork.StockMovementRepository.GetAllStockMovementsAsync();

            if (result is null)
                return ResultPattern<IList<StockMovement>?>.Failure("Não foi encontrado movimentaçoes.");

            return ResultPattern<IList<StockMovement>?>.Success(result);
        }
    }
}
