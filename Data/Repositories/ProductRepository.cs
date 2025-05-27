using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;

namespace Data.Repositories
{
    /// <summary>
    /// Product repository implementation with specialized product operations
    /// </summary>
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class ProductRepository : InMemoryRepository<Product>, IProductRepository
    {
        /// <summary>
        /// Gets products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>Products in the specified category</returns>
        public IEnumerable<Product> GetByCategory(ProductCategory category)
        {
            return Find(p => p.Category == category);
        }

        /// <summary>
        /// Gets products by name (case-insensitive partial match)
        /// </summary>
        /// <param name="name">The product name to search for</param>
        /// <returns>Products matching the name</returns>
        public IEnumerable<Product> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Enumerable.Empty<Product>();
            }

            var searchTerm = name.Trim().ToLowerInvariant();
            return Find(p => p.Name != null && p.Name.ToLowerInvariant().Contains(searchTerm));
        }

        /// <summary>
        /// Gets products by SKU (exact match)
        /// </summary>
        /// <param name="sku">The SKU to search for</param>
        /// <returns>Product with the specified SKU, or null if not found</returns>
        public Product GetBySKU(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return null;
            }

            var searchSKU = sku.Trim().ToUpperInvariant();
            return Find(p => p.SKU != null && p.SKU.ToUpperInvariant() == searchSKU).FirstOrDefault();
        }

        /// <summary>
        /// Gets products within a price range
        /// </summary>
        /// <param name="minPrice">Minimum price (inclusive)</param>
        /// <param name="maxPrice">Maximum price (inclusive)</param>
        /// <returns>Products within the price range</returns>
        public IEnumerable<Product> GetByPriceRange(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0)
            {
                throw new ArgumentException("Minimum price cannot be negative.", nameof(minPrice));
            }

            if (maxPrice < minPrice)
            {
                throw new ArgumentException("Maximum price cannot be less than minimum price.", nameof(maxPrice));
            }

            return Find(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        /// <summary>
        /// Gets active products only
        /// </summary>
        /// <returns>Active products</returns>
        public IEnumerable<Product> GetActiveProducts()
        {
            return Find(p => p.IsActive);
        }

        /// <summary>
        /// Gets products with low stock (below specified threshold)
        /// </summary>
        /// <param name="threshold">Stock threshold</param>
        /// <returns>Products with stock below threshold</returns>
        public IEnumerable<Product> GetLowStockProducts(int threshold = 10)
        {
            if (threshold < 0)
            {
                throw new ArgumentException("Threshold cannot be negative.", nameof(threshold));
            }

            return Find(p => p.StockQuantity < threshold);
        }

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
        public IEnumerable<Product> GetFiltered(
            string name = null,
            ProductCategory? category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null,
            int skip = 0,
            int take = 50)
        {
            if (skip < 0)
            {
                throw new ArgumentException("Skip cannot be negative.", nameof(skip));
            }

            if (take <= 0)
            {
                throw new ArgumentException("Take must be greater than zero.", nameof(take));
            }

            var query = GetAll().AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(name))
            {
                var searchTerm = name.Trim().ToLowerInvariant();
                query = query.Where(p => p.Name != null && p.Name.ToLowerInvariant().Contains(searchTerm));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.Category == category.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(p => p.IsActive == isActive.Value);
            }

            // Apply pagination
            return query.Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Gets the count of products matching the filter criteria
        /// </summary>
        /// <param name="name">Product name filter (optional)</param>
        /// <param name="category">Category filter (optional)</param>
        /// <param name="minPrice">Minimum price filter (optional)</param>
        /// <param name="maxPrice">Maximum price filter (optional)</param>
        /// <param name="isActive">Active status filter (optional)</param>
        /// <returns>Count of matching products</returns>
        public int GetFilteredCount(
            string name = null,
            ProductCategory? category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null)
        {
            var query = GetAll().AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(name))
            {
                var searchTerm = name.Trim().ToLowerInvariant();
                query = query.Where(p => p.Name != null && p.Name.ToLowerInvariant().Contains(searchTerm));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.Category == category.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(p => p.IsActive == isActive.Value);
            }

            return query.Count();
        }

        /// <summary>
        /// Checks if a SKU already exists
        /// </summary>
        /// <param name="sku">The SKU to check</param>
        /// <param name="excludeProductId">Product ID to exclude from the check (for updates)</param>
        /// <returns>True if SKU exists, false otherwise</returns>
        public bool SKUExists(string sku, Guid? excludeProductId = null)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return false;
            }

            var searchSKU = sku.Trim().ToUpperInvariant();

            if (excludeProductId.HasValue)
            {
                return Any(p => p.SKU != null &&
                               p.SKU.ToUpperInvariant() == searchSKU &&
                               p.Id != excludeProductId.Value);
            }

            return Any(p => p.SKU != null && p.SKU.ToUpperInvariant() == searchSKU);
        }
    }
}
