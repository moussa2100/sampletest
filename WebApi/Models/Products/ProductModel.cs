using BusinessEntities;

namespace WebApi.Models.Products
{
    /// <summary>
    /// Model for creating and updating products
    /// </summary>
    public class ProductModel
    {
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
        public ProductCategory Category { get; set; }

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
    }
}
