using ProductControl.Domain.Common;

namespace ProductControl.Domain.Entities
{
    public class StockMovement : EntitiyBase
    {
        public Guid ProductId { get; private set; }
        public Guid UserId { get; set; }
        public int Amount { get; private set; }

        private StockMovement(Guid productId, Guid userId, int amount)
        {
            ProductId = productId;
            UserId = userId;
            Amount = amount;
            CreatedAt = DateTime.UtcNow;
        }

        public static ResultPattern<StockMovement> Create(Guid productId,  Guid userId, int amount)
        {
            if (amount <= 0)
                return ResultPattern<StockMovement>.Failure("A quantidade movimentada não pode ser/menor que zero");

            return ResultPattern<StockMovement>.Success(new StockMovement(productId, userId, amount));
        }

        private StockMovement() { }
    }
}
