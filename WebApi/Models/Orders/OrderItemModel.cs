using System;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Model for order item information
    /// </summary>
    public class OrderItemModel
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}
