using ProductControl.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.Interfaces.Queries
{
    public interface IProductQueries
    {
        public Task<IList<ReturnProduct>?> GetAllProducts();
        public Task<ReturnProduct?> GetByIdProduct(Guid id);
    }
}
