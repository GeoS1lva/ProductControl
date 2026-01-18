namespace ProductControl.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public IUsersRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IStockMovementRepository StockMovementRepository { get; }

        public Task SaveChangesAsync();
    }
}
