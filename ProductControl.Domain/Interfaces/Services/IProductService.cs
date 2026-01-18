using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces.Services
{
    public interface IProductService
    {
        public Task<ResultPattern<Product>> CreateAsync(string name, string description, decimal price, int currentQuantity);
        public Task<ResultPattern> UpdateProfileAsync(Product product, string name, string description, decimal price);
    }
}
