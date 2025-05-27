using BusinessEntities;

namespace Core.Services.Products
{
    /// <summary>
    /// Interface for updating products
    /// </summary>
    public interface IUpdateProductService
    {
        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="name">Product name</param>
        /// <param name="description">Product description</param>
        /// <param name="price">Product price</param>
        /// <param name="category">Product category</param>
        /// <param name="sku">Stock Keeping Unit</param>
        /// <param name="stockQuantity">Stock quantity</param>
        /// <param name="isActive">Whether the product is active</param>
        void Update(Product product, string name, string description, decimal price, ProductCategory category, string sku, int stockQuantity, bool isActive);

        /// <summary>
        /// Updates product stock quantity
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="newQuantity">New stock quantity</param>
        void UpdateStock(Product product, int newQuantity);

        /// <summary>
        /// Adds stock to a product
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="quantity">Quantity to add</param>
        void AddStock(Product product, int quantity);

        /// <summary>
        /// Removes stock from a product
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="quantity">Quantity to remove</param>
        void RemoveStock(Product product, int quantity);
    }
}
