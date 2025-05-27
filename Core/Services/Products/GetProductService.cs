using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Products
{
    /// <summary>
    /// Service for retrieving products
    /// </summary>
    [AutoRegister]
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the GetProductService class
        /// </summary>
        /// <param name="productRepository">Product repository</param>
        public GetProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        /// <summary>
        /// Gets a product by ID
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>The product if found, null otherwise</returns>
        public Product GetProduct(Guid id)
        {
            return _productRepository.Get(id);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>All products</returns>
        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetAll();
        }

        /// <summary>
        /// Gets products with filtering
        /// </summary>
        /// <param name="name">Product name filter (optional)</param>
        /// <param name="category">Category filter (optional)</param>
        /// <param name="minPrice">Minimum price filter (optional)</param>
        /// <param name="maxPrice">Maximum price filter (optional)</param>
        /// <param name="isActive">Active status filter (optional)</param>
        /// <returns>Filtered products</returns>
        public IEnumerable<Product> GetProducts(string name = null, ProductCategory? category = null, decimal? minPrice = null, decimal? maxPrice = null, bool? isActive = null)
        {
            return _productRepository.GetFiltered(name, category, minPrice, maxPrice, isActive);
        }

        /// <summary>
        /// Gets products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>Products in the specified category</returns>
        public IEnumerable<Product> GetProductsByCategory(ProductCategory category)
        {
            return _productRepository.GetByCategory(category);
        }

        /// <summary>
        /// Gets a product by SKU
        /// </summary>
        /// <param name="sku">The SKU</param>
        /// <returns>The product if found, null otherwise</returns>
        public Product GetProductBySKU(string sku)
        {
            return _productRepository.GetBySKU(sku);
        }

        /// <summary>
        /// Gets products with low stock
        /// </summary>
        /// <param name="threshold">Stock threshold</param>
        /// <returns>Products with stock below threshold</returns>
        public IEnumerable<Product> GetLowStockProducts(int threshold = 10)
        {
            return _productRepository.GetLowStockProducts(threshold);
        }

        /// <summary>
        /// Gets active products only
        /// </summary>
        /// <returns>Active products</returns>
        public IEnumerable<Product> GetActiveProducts()
        {
            return _productRepository.GetActiveProducts();
        }

        /// <summary>
        /// Checks if a SKU exists
        /// </summary>
        /// <param name="sku">The SKU to check</param>
        /// <param name="excludeProductId">Product ID to exclude from check</param>
        /// <returns>True if SKU exists, false otherwise</returns>
        public bool SKUExists(string sku, Guid? excludeProductId = null)
        {
            return _productRepository.SKUExists(sku, excludeProductId);
        }
    }
}
