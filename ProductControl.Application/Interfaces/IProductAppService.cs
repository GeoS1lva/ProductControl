using ProductControl.Application.DTOs.ProductDTOs;
using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.Interfaces
{
    public interface IProductAppService
    {
        public Task<ResultPattern<Product>> CreateProductAsync(CreateProduct product);
        public Task<ResultPattern<IList<ReturnProduct>>> ReturnProductsAsync();
        public Task<ResultPattern<ReturnProduct>> ReturnProductAsync(Guid id);
        public Task<ResultPattern> UpdateProductAsync(Guid productId, UpdateProduct upProduct);
        public Task<ResultPattern> DesactiveProduct(Guid productId);
        public Task<ResultPattern> ActivateProduct(Guid productId);
        public Task<ResultPattern> AddProductAsync(Guid productId, Guid userId, int amount);
        public Task<ResultPattern> RemoveProductAsync(Guid productId, Guid userId, int amount);
    }
}
