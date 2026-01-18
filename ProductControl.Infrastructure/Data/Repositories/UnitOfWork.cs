using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Infrastructure.Data.Repositories
{
    public class UnitOfWork(PostgreDbContext context) : IUnitOfWork
    {
        public IUsersRepository UserRepository => new UsersRepository(context);
        public IProductRepository ProductRepository => new ProductRepository(context);
        public IStockMovementRepository StockMovementRepository => new StockMovementRepository(context);

        public async Task SaveChangesAsync()
            => await context.SaveChangesAsync();
    }
}
