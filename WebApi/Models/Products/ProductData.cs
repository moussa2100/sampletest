using System;
using BusinessEntities;

namespace WebApi.Models.Products
{
    /// <summary>
    /// Data transfer object for product responses
    /// </summary>
    public class ProductData : IdObjectData
    {
        /// <summary>
        /// Initializes a new instance of ProductData
        /// </summary>
        /// <param name="product">The product entity</param>
        public ProductData(Product product) : base(product)
        {
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Category = new EnumData(product.Category);
            SKU = product.SKU;
            StockQuantity = product.StockQuantity;
            IsActive = product.IsActive;
            CreatedDate = product.CreatedDate;
            UpdatedDate = product.UpdatedDate;
        }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        public EnumData Category { get; set; }

        /// <summary>
        /// Stock Keeping Unit
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Whether the product is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Date when the product was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the product was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }
    }
}
