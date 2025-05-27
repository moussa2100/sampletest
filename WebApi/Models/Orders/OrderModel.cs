using System;
using System.Collections.Generic;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Model for creating and updating orders
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Customer name
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Customer email
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Customer ID (optional)
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Shipping address
        /// </summary>
        public AddressModel ShippingAddress { get; set; }

        /// <summary>
        /// Order notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Order items (for create with items)
        /// </summary>
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
    }
}
