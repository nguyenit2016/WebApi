using Microsoft.EntityFrameworkCore;
using OrderServices.Helpers;
using OrderServices.Models;
using OrderServices.Repositories;
using OrderServices.UnitOfWork;
using OrderServices.ViewModels;

namespace OrderServices.Services
{
    public interface IProductService
    {
        ApiResponse<List<ProductResponse>> GetAll();
        Task<ApiResponse<ProductResponse>> AddAsync(ProductRequest product);
        Task<ApiResponse<ProductResponse>> UpdateAsync(int productId, ProductRequest product);
        Task<ApiResponse<ProductResponse>> DeleteAsync(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly OrderDbContext _context;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, OrderDbContext context)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public ApiResponse<List<ProductResponse>> GetAll()
        {
            var res = _context.Set<Product>()
                .Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price
                })
                .AsNoTracking()
                .ToList();
            return ApiResponse<List<ProductResponse>>.SuccessResponse(res);
        }

        public Task<ApiResponse<ProductResponse>> AddAsync(ProductRequest product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price
            };
            _productRepository.Add(newProduct);
            var result = _unitOfWork.SaveChangesAsync().Result;
            if (result <= 0)
            {
                return Task.FromResult(ApiResponse<ProductResponse>.FailureResponse("Failed to create product."));
            }
            var response = new ProductResponse
            {
                ProductId = newProduct.ProductId,
                Name = newProduct.Name,
                Price = newProduct.Price
            };
            return Task.FromResult(ApiResponse<ProductResponse>.SuccessResponse(response));
        }

        public async Task<ApiResponse<ProductResponse>> UpdateAsync(int productId, ProductRequest product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                return ApiResponse<ProductResponse>.FailureResponse("Product not found.");
            }
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            _productRepository.Update(existingProduct);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ApiResponse<ProductResponse>.FailureResponse("Failed to update product.");
            }
            var response = new ProductResponse
            {
                ProductId = existingProduct.ProductId,
                Name = existingProduct.Name,
                Price = existingProduct.Price
            };
            return ApiResponse<ProductResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<ProductResponse>> DeleteAsync(int productId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                return ApiResponse<ProductResponse>.FailureResponse("Product not found.");
            }
            _productRepository.Delete(existingProduct);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ApiResponse<ProductResponse>.FailureResponse("Failed to delete product.");
            }
            return ApiResponse<ProductResponse>.SuccessResponse(new ProductResponse
            {
                ProductId = existingProduct.ProductId,
                Name = existingProduct.Name,
                Price = existingProduct.Price
            });
        }
    }
}
