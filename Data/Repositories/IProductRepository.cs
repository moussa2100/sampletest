using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Data.Repositories
{
    /// <summary>
    /// Interface for product-specific repository operations
    /// </summary>
    public interface IProductRepository : IInMemoryRepository<Product>
    {
        /// <summary>
        /// Gets products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>Products in the specified category</returns>
        IEnumerable<Product> GetByCategory(ProductCategory category);

        /// <summary>
        /// Gets products by name (case-insensitive partial match)
        /// </summary>
        /// <param name="name">The product name to search for</param>
        /// <returns>Products matching the name</returns>
        IEnumerable<Product> GetByName(string name);

        /// <summary>
        /// Gets products by SKU (exact match)
        /// </summary>
        /// <param name="sku">The SKU to search for</param>
        /// <returns>Product with the specified SKU, or null if not found</returns>
        Product GetBySKU(string sku);

        /// <summary>
        /// Gets products within a price range
        /// </summary>
        /// <param name="minPrice">Minimum price (inclusive)</param>
        /// <param name="maxPrice">Maximum price (inclusive)</param>
        /// <returns>Products within the price range</returns>
        IEnumerable<Product> GetByPriceRange(decimal minPrice, decimal maxPrice);

        /// <summary>
        /// Gets active products only
        /// </summary>
        /// <returns>Active products</returns>
        IEnumerable<Product> GetActiveProducts();

        /// <summary>
        /// Gets products with low stock (below specified threshold)
        /// </summary>
        /// <param name="threshold">Stock threshold</param>
        /// <returns>Products with stock below threshold</returns>
        IEnumerable<Product> GetLowStockProducts(int threshold = 10);

        /// <summary>
        /// Gets products with advanced filtering
        /// </summary>
        /// <param name="name">Product name filter (optional)</param>
        /// <param name="category">Category filter (optional)</param>
        /// <param name="minPrice">Minimum price filter (optional)</param>
        /// <param name="maxPrice">Maximum price filter (optional)</param>
        /// <param name="isActive">Active status filter (optional)</param>
        /// <param name="skip">Number of products to skip for pagination</param>
        /// <param name="take">Number of products to take for pagination</param>
        /// <returns>Filtered and paginated products</returns>
        IEnumerable<Product> GetFiltered(
            string name = null,
            ProductCategory? category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null,
            int skip = 0,
            int take = 50);

        /// <summary>
        /// Gets the count of products matching the filter criteria
        /// </summary>
        /// <param name="name">Product name filter (optional)</param>
        /// <param name="category">Category filter (optional)</param>
        /// <param name="minPrice">Minimum price filter (optional)</param>
        /// <param name="maxPrice">Maximum price filter (optional)</param>
        /// <param name="isActive">Active status filter (optional)</param>
        /// <returns>Count of matching products</returns>
        int GetFilteredCount(
            string name = null,
            ProductCategory? category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null);

        /// <summary>
        /// Checks if a SKU already exists
        /// </summary>
        /// <param name="sku">The SKU to check</param>
        /// <param name="excludeProductId">Product ID to exclude from the check (for updates)</param>
        /// <returns>True if SKU exists, false otherwise</returns>
        bool SKUExists(string sku, Guid? excludeProductId = null);
    }
}
