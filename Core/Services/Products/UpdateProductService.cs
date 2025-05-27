using System;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Products
{
    /// <summary>
    /// Service for updating products
    /// </summary>
    [AutoRegister]
    public class UpdateProductService : IUpdateProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the UpdateProductService class
        /// </summary>
        /// <param name="productRepository">Product repository</param>
        public UpdateProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

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
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        /// <exception cref="ArgumentException">Thrown when SKU already exists for another product</exception>
        public void Update(Product product, string name, string description, decimal price, ProductCategory category, string sku, int stockQuantity, bool isActive)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            // Check if SKU already exists for another product
            if (_productRepository.SKUExists(sku, product.Id))
            {
                throw new ArgumentException($"A product with SKU '{sku}' already exists.", nameof(sku));
            }

            product.SetName(name);
            product.SetDescription(description);
            product.SetPrice(price);
            product.SetCategory(category);
            product.SetSKU(sku);
            product.SetStockQuantity(stockQuantity);
            product.SetActiveStatus(isActive);

            _productRepository.Save(product);
        }

        /// <summary>
        /// Updates product stock quantity
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="newQuantity">New stock quantity</param>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        public void UpdateStock(Product product, int newQuantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.SetStockQuantity(newQuantity);
            _productRepository.Save(product);
        }

        /// <summary>
        /// Adds stock to a product
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="quantity">Quantity to add</param>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        public void AddStock(Product product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.AddStock(quantity);
            _productRepository.Save(product);
        }

        /// <summary>
        /// Removes stock from a product
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="quantity">Quantity to remove</param>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        public void RemoveStock(Product product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.RemoveStock(quantity);
            _productRepository.Save(product);
        }
    }
}
