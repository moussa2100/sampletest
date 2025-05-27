using System;

namespace BusinessEntities
{
    /// <summary>
    /// Represents a product entity with comprehensive business properties
    /// </summary>
    public class Product : IdObject
    {
        private string _name;
        private string _description;
        private decimal _price;
        private ProductCategory _category;
        private string _sku;
        private bool _isActive;
        private int _stockQuantity;
        private DateTime _createdDate;
        private DateTime _updatedDate;

        /// <summary>
        /// Initializes a new instance of the Product class
        /// </summary>
        public Product()
        {
            _createdDate = DateTime.UtcNow;
            _updatedDate = DateTime.UtcNow;
            _isActive = true;
            _stockQuantity = 0;
            _category = ProductCategory.Other;
        }

        /// <summary>
        /// Gets the product name
        /// </summary>
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        /// <summary>
        /// Gets the product description
        /// </summary>
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        /// <summary>
        /// Gets the product price
        /// </summary>
        public decimal Price
        {
            get => _price;
            private set => _price = value;
        }

        /// <summary>
        /// Gets the product category
        /// </summary>
        public ProductCategory Category
        {
            get => _category;
            private set => _category = value;
        }

        /// <summary>
        /// Gets the Stock Keeping Unit (SKU) identifier
        /// </summary>
        public string SKU
        {
            get => _sku;
            private set => _sku = value;
        }

        /// <summary>
        /// Gets whether the product is active and available for sale
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            private set => _isActive = value;
        }

        /// <summary>
        /// Gets the current stock quantity
        /// </summary>
        public int StockQuantity
        {
            get => _stockQuantity;
            private set => _stockQuantity = value;
        }

        /// <summary>
        /// Gets the date when the product was created
        /// </summary>
        public DateTime CreatedDate
        {
            get => _createdDate;
            private set => _createdDate = value;
        }

        /// <summary>
        /// Gets the date when the product was last updated
        /// </summary>
        public DateTime UpdatedDate
        {
            get => _updatedDate;
            private set => _updatedDate = value;
        }

        /// <summary>
        /// Sets the product name with validation
        /// </summary>
        /// <param name="name">The product name</param>
        /// <exception cref="ArgumentException">Thrown when name is null or empty</exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            }

            if (name.Length > 200)
            {
                throw new ArgumentException("Product name cannot exceed 200 characters.", nameof(name));
            }

            _name = name.Trim();
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the product description
        /// </summary>
        /// <param name="description">The product description</param>
        public void SetDescription(string description)
        {
            if (description != null && description.Length > 1000)
            {
                throw new ArgumentException("Product description cannot exceed 1000 characters.", nameof(description));
            }

            _description = description != null ? description.Trim() : null;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the product price with validation
        /// </summary>
        /// <param name="price">The product price</param>
        /// <exception cref="ArgumentException">Thrown when price is negative</exception>
        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Product price cannot be negative.", nameof(price));
            }

            _price = price;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the product category
        /// </summary>
        /// <param name="category">The product category</param>
        public void SetCategory(ProductCategory category)
        {
            _category = category;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the SKU with validation
        /// </summary>
        /// <param name="sku">The Stock Keeping Unit identifier</param>
        /// <exception cref="ArgumentException">Thrown when SKU is invalid</exception>
        public void SetSKU(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("SKU cannot be null or empty.", nameof(sku));
            }

            if (sku.Length > 50)
            {
                throw new ArgumentException("SKU cannot exceed 50 characters.", nameof(sku));
            }

            _sku = sku.Trim().ToUpperInvariant();
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the active status of the product
        /// </summary>
        /// <param name="isActive">Whether the product is active</param>
        public void SetActiveStatus(bool isActive)
        {
            _isActive = isActive;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the stock quantity with validation
        /// </summary>
        /// <param name="quantity">The stock quantity</param>
        /// <exception cref="ArgumentException">Thrown when quantity is negative</exception>
        public void SetStockQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.", nameof(quantity));
            }

            _stockQuantity = quantity;
            UpdateTimestamp();
        }

        /// <summary>
        /// Adds stock to the current quantity
        /// </summary>
        /// <param name="quantity">The quantity to add</param>
        /// <exception cref="ArgumentException">Thrown when quantity is negative or zero</exception>
        public void AddStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity to add must be positive.", nameof(quantity));
            }

            _stockQuantity += quantity;
            UpdateTimestamp();
        }

        /// <summary>
        /// Removes stock from the current quantity
        /// </summary>
        /// <param name="quantity">The quantity to remove</param>
        /// <exception cref="ArgumentException">Thrown when quantity is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when insufficient stock</exception>
        public void RemoveStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity to remove must be positive.", nameof(quantity));
            }

            if (_stockQuantity < quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {_stockQuantity}, Requested: {quantity}");
            }

            _stockQuantity -= quantity;
            UpdateTimestamp();
        }

        /// <summary>
        /// Checks if the product is available for purchase
        /// </summary>
        /// <param name="requestedQuantity">The requested quantity</param>
        /// <returns>True if available, false otherwise</returns>
        public bool IsAvailable(int requestedQuantity = 1)
        {
            return _isActive && _stockQuantity >= requestedQuantity;
        }

        /// <summary>
        /// Updates the timestamp to current UTC time
        /// </summary>
        private void UpdateTimestamp()
        {
            _updatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Returns a string representation of the product
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"Product: {_name} (SKU: {_sku}, Price: ${_price:F2}, Stock: {_stockQuantity})";
        }
    }
}
