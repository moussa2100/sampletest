using System;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Products
{
    /// <summary>
    /// Service for deleting products
    /// </summary>
    [AutoRegister]
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteProductService class
        /// </summary>
        /// <param name="productRepository">Product repository</param>
        public DeleteProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="product">The product to delete</param>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        public void Delete(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _productRepository.Delete(product);
        }

        /// <summary>
        /// Deletes all products (for testing/cleanup purposes)
        /// </summary>
        public void DeleteAll()
        {
            _productRepository.Clear();
        }
    }
}
