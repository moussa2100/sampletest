using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessEntities
{
    /// <summary>
    /// Represents an order entity with comprehensive business properties
    /// </summary>
    public class Order : IdObject
    {
        private string _orderNumber;
        private Guid? _customerId;
        private string _customerName;
        private string _customerEmail;
        private DateTime _orderDate;
        private OrderStatus _status;
        private decimal _totalAmount;
        private readonly List<OrderItem> _orderItems;
        private Address _shippingAddress;
        private DateTime _createdDate;
        private DateTime _updatedDate;
        private string _notes;

        /// <summary>
        /// Initializes a new instance of the Order class
        /// </summary>
        public Order()
        {
            _orderItems = new List<OrderItem>();
            _orderDate = DateTime.UtcNow;
            _createdDate = DateTime.UtcNow;
            _updatedDate = DateTime.UtcNow;
            _status = OrderStatus.Pending;
            _totalAmount = 0;
            GenerateOrderNumber();
        }

        /// <summary>
        /// Gets the unique order number
        /// </summary>
        public string OrderNumber
        {
            get => _orderNumber;
            private set => _orderNumber = value;
        }

        /// <summary>
        /// Gets the customer ID (optional for guest orders)
        /// </summary>
        public Guid? CustomerId
        {
            get => _customerId;
            private set => _customerId = value;
        }

        /// <summary>
        /// Gets the customer name
        /// </summary>
        public string CustomerName
        {
            get => _customerName;
            private set => _customerName = value;
        }

        /// <summary>
        /// Gets the customer email
        /// </summary>
        public string CustomerEmail
        {
            get => _customerEmail;
            private set => _customerEmail = value;
        }

        /// <summary>
        /// Gets the order date
        /// </summary>
        public DateTime OrderDate
        {
            get => _orderDate;
            private set => _orderDate = value;
        }

        /// <summary>
        /// Gets the order status
        /// </summary>
        public OrderStatus Status
        {
            get => _status;
            private set => _status = value;
        }

        /// <summary>
        /// Gets the total amount of the order
        /// </summary>
        public decimal TotalAmount
        {
            get => _totalAmount;
            private set => _totalAmount = value;
        }

        /// <summary>
        /// Gets the order items
        /// </summary>
        public IReadOnlyList<OrderItem> OrderItems
        {
            get => _orderItems.AsReadOnly();
        }

        /// <summary>
        /// Gets the shipping address
        /// </summary>
        public Address ShippingAddress
        {
            get => _shippingAddress;
            private set => _shippingAddress = value;
        }

        /// <summary>
        /// Gets the date when the order was created
        /// </summary>
        public DateTime CreatedDate
        {
            get => _createdDate;
            private set => _createdDate = value;
        }

        /// <summary>
        /// Gets the date when the order was last updated
        /// </summary>
        public DateTime UpdatedDate
        {
            get => _updatedDate;
            private set => _updatedDate = value;
        }

        /// <summary>
        /// Gets the order notes
        /// </summary>
        public string Notes
        {
            get => _notes;
            private set => _notes = value;
        }

        /// <summary>
        /// Sets the customer information
        /// </summary>
        /// <param name="customerName">The customer name</param>
        /// <param name="customerEmail">The customer email</param>
        /// <param name="customerId">The customer ID (optional)</param>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid</exception>
        public void SetCustomer(string customerName, string customerEmail, Guid? customerId = null)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new ArgumentException("Customer name cannot be null or empty.", nameof(customerName));
            }

            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                throw new ArgumentException("Customer email cannot be null or empty.", nameof(customerEmail));
            }

            if (!IsValidEmail(customerEmail))
            {
                throw new ArgumentException("Customer email is not in a valid format.", nameof(customerEmail));
            }

            _customerName = customerName.Trim();
            _customerEmail = customerEmail.Trim().ToLowerInvariant();
            _customerId = customerId;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the shipping address
        /// </summary>
        /// <param name="address">The shipping address</param>
        /// <exception cref="ArgumentNullException">Thrown when address is null</exception>
        /// <exception cref="ArgumentException">Thrown when address is invalid</exception>
        public void SetShippingAddress(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (!address.IsValid())
            {
                throw new ArgumentException("Address is not valid. All fields are required.", nameof(address));
            }

            _shippingAddress = address;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the order status
        /// </summary>
        /// <param name="status">The new status</param>
        public void SetStatus(OrderStatus status)
        {
            _status = status;
            UpdateTimestamp();
        }

        /// <summary>
        /// Sets the order notes
        /// </summary>
        /// <param name="notes">The order notes</param>
        public void SetNotes(string notes)
        {
            if (notes != null && notes.Length > 1000)
            {
                throw new ArgumentException("Notes cannot exceed 1000 characters.", nameof(notes));
            }

            _notes = notes != null ? notes.Trim() : null;
            UpdateTimestamp();
        }

        /// <summary>
        /// Adds an item to the order
        /// </summary>
        /// <param name="orderItem">The order item to add</param>
        /// <exception cref="ArgumentNullException">Thrown when orderItem is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when order cannot be modified</exception>
        public void AddItem(OrderItem orderItem)
        {
            if (orderItem == null)
            {
                throw new ArgumentNullException(nameof(orderItem));
            }

            if (!CanModifyItems())
            {
                throw new InvalidOperationException($"Cannot modify items when order status is {_status}.");
            }

            // Check if item with same product already exists
            var existingItem = _orderItems.FirstOrDefault(item => item.ProductId == orderItem.ProductId);
            if (existingItem != null)
            {
                // Update quantity of existing item
                existingItem.SetQuantity(existingItem.Quantity + orderItem.Quantity);
            }
            else
            {
                _orderItems.Add(orderItem);
            }

            CalculateTotalAmount();
            UpdateTimestamp();
        }

        /// <summary>
        /// Removes an item from the order
        /// </summary>
        /// <param name="productId">The product ID to remove</param>
        /// <exception cref="InvalidOperationException">Thrown when order cannot be modified</exception>
        public void RemoveItem(Guid productId)
        {
            if (!CanModifyItems())
            {
                throw new InvalidOperationException($"Cannot modify items when order status is {_status}.");
            }

            var itemToRemove = _orderItems.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                _orderItems.Remove(itemToRemove);
                CalculateTotalAmount();
                UpdateTimestamp();
            }
        }

        /// <summary>
        /// Updates the quantity of an existing item
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="newQuantity">The new quantity</param>
        /// <exception cref="ArgumentException">Thrown when quantity is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when order cannot be modified or item not found</exception>
        public void UpdateItemQuantity(Guid productId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
            }

            if (!CanModifyItems())
            {
                throw new InvalidOperationException($"Cannot modify items when order status is {_status}.");
            }

            var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found in order.");
            }

            item.SetQuantity(newQuantity);
            CalculateTotalAmount();
            UpdateTimestamp();
        }

        /// <summary>
        /// Clears all items from the order
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when order cannot be modified</exception>
        public void ClearItems()
        {
            if (!CanModifyItems())
            {
                throw new InvalidOperationException($"Cannot modify items when order status is {_status}.");
            }

            _orderItems.Clear();
            CalculateTotalAmount();
            UpdateTimestamp();
        }

        /// <summary>
        /// Gets the total number of items in the order
        /// </summary>
        /// <returns>Total item count</returns>
        public int GetTotalItemCount()
        {
            return _orderItems.Sum(item => item.Quantity);
        }

        /// <summary>
        /// Checks if the order can be cancelled
        /// </summary>
        /// <returns>True if cancellable, false otherwise</returns>
        public bool CanBeCancelled()
        {
            return _status == OrderStatus.Pending || _status == OrderStatus.Processing;
        }

        /// <summary>
        /// Cancels the order
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when order cannot be cancelled</exception>
        public void Cancel()
        {
            if (!CanBeCancelled())
            {
                throw new InvalidOperationException($"Cannot cancel order with status {_status}.");
            }

            SetStatus(OrderStatus.Cancelled);
        }

        /// <summary>
        /// Generates a unique order number
        /// </summary>
        private void GenerateOrderNumber()
        {
            _orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Id.ToString("N").Substring(0, 8).ToUpperInvariant()}";
        }

        /// <summary>
        /// Calculates the total amount based on order items
        /// </summary>
        private void CalculateTotalAmount()
        {
            _totalAmount = _orderItems.Sum(item => item.TotalPrice);
        }

        /// <summary>
        /// Checks if order items can be modified based on current status
        /// </summary>
        /// <returns>True if items can be modified, false otherwise</returns>
        private bool CanModifyItems()
        {
            return _status == OrderStatus.Pending;
        }

        /// <summary>
        /// Updates the timestamp to current UTC time
        /// </summary>
        private void UpdateTimestamp()
        {
            _updatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        /// <param name="email">The email to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a string representation of the order
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"Order {_orderNumber}: {_customerName} - ${_totalAmount:F2} ({_status})";
        }
    }
}
