using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.DomainServices
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ResultPattern<Product>> CreateAsync(string name, string description, decimal price, int currentQuantity)
        {
            var resultValidate = await unitOfWork.ProductRepository.ValidateRegisteredName(name);

            if (resultValidate is true)
                return ResultPattern<Product>.Failure("Já existe um produto com esse nome.");

            var resultCreateProduct = Product.Create(name, description, price, currentQuantity);

            if (resultCreateProduct.Error)
                return ResultPattern<Product>.Failure(resultCreateProduct.ErrorMessage);

            return ResultPattern<Product>.Success(resultCreateProduct.Value);
        }

        public async Task<ResultPattern> UpdateProfileAsync(Product product, string name, string description, decimal price)
        {
            if (product.Name != name)
            {
                var resultValidate = await unitOfWork.ProductRepository.ValidateRegisteredName(name);

                if (resultValidate is true)
                    return ResultPattern.Failure("Já existe um produto com esse nome.");
            }

            var resultUpdate =  product.UpdateProfile(name, description, price);

            if (resultUpdate.Error)
                return ResultPattern.Failure(resultUpdate.ErrorMessage);

            return ResultPattern.Success();
        }
    }
}
