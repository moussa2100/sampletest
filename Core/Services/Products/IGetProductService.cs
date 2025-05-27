using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Products
{
    /// <summary>
    /// Interface for retrieving products
    /// </summary>
    public interface IGetProductService
    {
        /// <summary>
        /// Gets a product by ID
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>The product if found, null otherwise</returns>
        Product GetProduct(Guid id);

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>All products</returns>
        IEnumerable<Product> GetProducts();

        /// <summary>
        /// Gets products with filtering
        /// </summary>
        /// <param name="name">Product name filter (optional)</param>
        /// <param name="category">Category filter (optional)</param>
        /// <param name="minPrice">Minimum price filter (optional)</param>
        /// <param name="maxPrice">Maximum price filter (optional)</param>
        /// <param name="isActive">Active status filter (optional)</param>
        /// <returns>Filtered products</returns>
        IEnumerable<Product> GetProducts(string name = null, ProductCategory? category = null, decimal? minPrice = null, decimal? maxPrice = null, bool? isActive = null);

        /// <summary>
        /// Gets products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>Products in the specified category</returns>
        IEnumerable<Product> GetProductsByCategory(ProductCategory category);

        /// <summary>
        /// Gets a product by SKU
        /// </summary>
        /// <param name="sku">The SKU</param>
        /// <returns>The product if found, null otherwise</returns>
        Product GetProductBySKU(string sku);

        /// <summary>
        /// Gets products with low stock
        /// </summary>
        /// <param name="threshold">Stock threshold</param>
        /// <returns>Products with stock below threshold</returns>
        IEnumerable<Product> GetLowStockProducts(int threshold = 10);

        /// <summary>
        /// Gets active products only
        /// </summary>
        /// <returns>Active products</returns>
        IEnumerable<Product> GetActiveProducts();

        /// <summary>
        /// Checks if a SKU exists
        /// </summary>
        /// <param name="sku">The SKU to check</param>
        /// <param name="excludeProductId">Product ID to exclude from check</param>
        /// <returns>True if SKU exists, false otherwise</returns>
        bool SKUExists(string sku, Guid? excludeProductId = null);
    }
}
