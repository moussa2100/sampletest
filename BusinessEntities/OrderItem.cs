using System;

namespace BusinessEntities
{
    /// <summary>
    /// Represents an item within an order
    /// </summary>
    public class OrderItem
    {
        private Guid _productId;
        private string _productName;
        private string _productSKU;
        private int _quantity;
        private decimal _unitPrice;
        private decimal _totalPrice;

        /// <summary>
        /// Initializes a new instance of the OrderItem class
        /// </summary>
        public OrderItem()
        {
        }

        /// <summary>
        /// Gets the product ID
        /// </summary>
        public Guid ProductId
        {
            get => _productId;
            private set => _productId = value;
        }

        /// <summary>
        /// Gets the product name at the time of order
        /// </summary>
        public string ProductName
        {
            get => _productName;
            private set => _productName = value;
        }

        /// <summary>
        /// Gets the product SKU at the time of order
        /// </summary>
        public string ProductSKU
        {
            get => _productSKU;
            private set => _productSKU = value;
        }

        /// <summary>
        /// Gets the quantity ordered
        /// </summary>
        public int Quantity
        {
            get => _quantity;
            private set => _quantity = value;
        }

        /// <summary>
        /// Gets the unit price at the time of order
        /// </summary>
        public decimal UnitPrice
        {
            get => _unitPrice;
            private set => _unitPrice = value;
        }

        /// <summary>
        /// Gets the total price for this line item
        /// </summary>
        public decimal TotalPrice
        {
            get => _totalPrice;
            private set => _totalPrice = value;
        }

        /// <summary>
        /// Sets the product information
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="productName">The product name</param>
        /// <param name="productSKU">The product SKU</param>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid</exception>
        public void SetProduct(Guid productId, string productName, string productSKU)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
            }

            if (string.IsNullOrWhiteSpace(productSKU))
            {
                throw new ArgumentException("Product SKU cannot be null or empty.", nameof(productSKU));
            }

            _productId = productId;
            _productName = productName.Trim();
            _productSKU = productSKU.Trim();
        }

        /// <summary>
        /// Sets the quantity with validation
        /// </summary>
        /// <param name="quantity">The quantity</param>
        /// <exception cref="ArgumentException">Thrown when quantity is invalid</exception>
        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            _quantity = quantity;
            CalculateTotalPrice();
        }

        /// <summary>
        /// Sets the unit price with validation
        /// </summary>
        /// <param name="unitPrice">The unit price</param>
        /// <exception cref="ArgumentException">Thrown when unit price is invalid</exception>
        public void SetUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0)
            {
                throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
            }

            _unitPrice = unitPrice;
            CalculateTotalPrice();
        }

        /// <summary>
        /// Creates an order item from a product
        /// </summary>
        /// <param name="product">The product</param>
        /// <param name="quantity">The quantity</param>
        /// <returns>A new OrderItem instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when product is null</exception>
        /// <exception cref="ArgumentException">Thrown when quantity is invalid</exception>
        public static OrderItem FromProduct(Product product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            var orderItem = new OrderItem();
            orderItem.SetProduct(product.Id, product.Name, product.SKU);
            orderItem.SetUnitPrice(product.Price);
            orderItem.SetQuantity(quantity);

            return orderItem;
        }

        /// <summary>
        /// Calculates the total price based on quantity and unit price
        /// </summary>
        private void CalculateTotalPrice()
        {
            _totalPrice = _quantity * _unitPrice;
        }

        /// <summary>
        /// Returns a string representation of the order item
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{_productName} (SKU: {_productSKU}) - Qty: {_quantity} x ${_unitPrice:F2} = ${_totalPrice:F2}";
        }
    }
}
