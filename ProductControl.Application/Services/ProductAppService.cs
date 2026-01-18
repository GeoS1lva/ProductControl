using ProductControl.Application.DTOs.ProductDTOs;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces;
using ProductControl.Application.Interfaces.Queries;
using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Domain.Interfaces.Services;

namespace ProductControl.Application.Services
{
    public class ProductAppService(IUnitOfWork unitOfWork, IProductService productService, IProductQueries productQueries) : IProductAppService
    {
        public async Task<ResultPattern<Product>> CreateProductAsync(CreateProduct product)
        {
            var productResult = await productService.CreateAsync(product.Name, product.Description, product.Price, product.CurrentQuantity);

            if (productResult.Error)
                return ResultPattern<Product>.Failure(productResult.ErrorMessage);

            await unitOfWork.ProductRepository.AddAsync(productResult.Value);
            await unitOfWork.SaveChangesAsync();

            return ResultPattern<Product>.Success(productResult.Value);
        }

        public async Task<ResultPattern<IList<ReturnProduct>>> ReturnProductsAsync()
        {
            var resultProducts = await productQueries.GetAllProducts();

            if (resultProducts is null)
                return ResultPattern<IList<ReturnProduct>>.Failure("Nenhum produto ativo encontrado.");

            return ResultPattern<IList<ReturnProduct>>.Success(resultProducts);
        }

        public async Task<ResultPattern<ReturnProduct>> ReturnProductAsync(Guid id)
        {
            var resultProducts = await productQueries.GetByIdProduct(id);

            if (resultProducts is null)
                return ResultPattern<ReturnProduct>.Failure("produto não encontrado.");

            return ResultPattern<ReturnProduct>.Success(resultProducts);
        }

        public async Task<ResultPattern> UpdateProductAsync(Guid productId, UpdateProduct upProduct)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product is null)
                return ResultPattern.Failure("Produto não encontrado.");

            var resultProfile = await productService.UpdateProfileAsync(product, upProduct.Name, upProduct.Description, upProduct.Price);

            if (resultProfile.Error)
                return ResultPattern.Failure(resultProfile.ErrorMessage);

            unitOfWork.ProductRepository.UpdateAsync(product);
            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern> DesactiveProduct(Guid productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product is null)
                return ResultPattern.Failure("Produto não encontrado!");

            var result = product.DeactivateProduct();

            if (result.Error)
                return ResultPattern.Failure(result.ErrorMessage);

            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern> ActivateProduct(Guid productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product is null)
                return ResultPattern.Failure("Produto não encontrado!");

            var result = product.ActivateProduct();

            if (result.Error)
                return ResultPattern.Failure(result.ErrorMessage);

            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern> AddProductAsync(Guid productId, Guid userId, int amount)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product is null)
                return ResultPattern.Failure("Produto não encontrado!");

            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
                return ResultPattern.Failure("Usuário não encontrado!");

            var stockMovement = StockMovement.Create(productId, userId, amount);

            if (stockMovement.Error)
                return ResultPattern.Failure(stockMovement.ErrorMessage);

            var resultMovement = product.AddProduct(amount, stockMovement.Value);

            if (resultMovement.Error)
                return ResultPattern.Failure(resultMovement.ErrorMessage);

            unitOfWork.ProductRepository.UpdateAsync(product);
            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern> RemoveProductAsync(Guid productId, Guid userId, int amount)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product is null)
                return ResultPattern.Failure("Produto não encontrado!");

            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
                return ResultPattern.Failure("Usuário não encontrado!");

            var stockMovement = StockMovement.Create(productId, userId, amount);

            if (stockMovement.Error)
                return ResultPattern.Failure(stockMovement.ErrorMessage);

            var resultMovement = product.RemoveProduct(amount, stockMovement.Value);

            if (resultMovement.Error)
                return ResultPattern.Failure(resultMovement.ErrorMessage);

            unitOfWork.ProductRepository.UpdateAsync(product);
            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }
    }
}
