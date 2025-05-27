using System;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;

namespace Core.Services.Products
{
    /// <summary>
    /// Service for creating products
    /// </summary>
    [AutoRegister]
    public class CreateProductService : ICreateProductService
    {
        private readonly IUpdateProductService _updateProductService;
        private readonly IIdObjectFactory<Product> _productFactory;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the CreateProductService class
        /// </summary>
        /// <param name="productFactory">Product factory</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="updateProductService">Update product service</param>
        public CreateProductService(IIdObjectFactory<Product> productFactory, IProductRepository productRepository, IUpdateProductService updateProductService)
        {
            _productFactory = productFactory ?? throw new ArgumentNullException(nameof(productFactory));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _updateProductService = updateProductService ?? throw new ArgumentNullException(nameof(updateProductService));
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <param name="name">Product name</param>
        /// <param name="description">Product description</param>
        /// <param name="price">Product price</param>
        /// <param name="category">Product category</param>
        /// <param name="sku">Stock Keeping Unit</param>
        /// <param name="stockQuantity">Initial stock quantity</param>
        /// <param name="isActive">Whether the product is active</param>
        /// <returns>The created product</returns>
        /// <exception cref="ArgumentException">Thrown when SKU already exists</exception>
        public Product Create(Guid id, string name, string description, decimal price, ProductCategory category, string sku, int stockQuantity, bool isActive = true)
        {
            // Check if SKU already exists
            if (_productRepository.SKUExists(sku))
            {
                throw new ArgumentException($"A product with SKU '{sku}' already exists.", nameof(sku));
            }

            var product = _productFactory.Create(id);
            _updateProductService.Update(product, name, description, price, category, sku, stockQuantity, isActive);
            _productRepository.Save(product);
            return product;
        }
    }
}
