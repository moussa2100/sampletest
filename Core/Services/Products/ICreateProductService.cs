using System;
using BusinessEntities;

namespace Core.Services.Products
{
    /// <summary>
    /// Interface for creating products
    /// </summary>
    public interface ICreateProductService
    {
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
        Product Create(Guid id, string name, string description, decimal price, ProductCategory category, string sku, int stockQuantity, bool isActive = true);
    }
}
