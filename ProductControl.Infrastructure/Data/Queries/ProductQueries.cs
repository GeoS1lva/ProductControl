using Microsoft.EntityFrameworkCore;
using ProductControl.Application.DTOs.ProductDTOs;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces.Queries;
using ProductControl.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Infrastructure.Data.Queries
{
    public class ProductQueries(PostgreDbContext postgreDbContext) : IProductQueries
    {
        public async Task<IList<ReturnProduct>?> GetAllProducts()
            => await postgreDbContext.Products.AsNoTracking()
            .Where(p => p.Status == true)
            .Select(product => new ReturnProduct
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CurrentQuantity = product.CurrentQuantity,
                CreateAt = product.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToListAsync();

        public async Task<ReturnProduct?> GetByIdProduct(Guid id)
            => await postgreDbContext.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(product => new ReturnProduct
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CurrentQuantity = product.CurrentQuantity,
                CreateAt = product.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).FirstOrDefaultAsync();
    }
}
