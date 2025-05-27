using System;
using BusinessEntities;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Data transfer object for order item responses
    /// </summary>
    public class OrderItemData
    {
        /// <summary>
        /// Initializes a new instance of OrderItemData
        /// </summary>
        /// <param name="orderItem">The order item entity</param>
        public OrderItemData(OrderItem orderItem)
        {
            ProductId = orderItem.ProductId;
            ProductName = orderItem.ProductName;
            ProductSKU = orderItem.ProductSKU;
            Quantity = orderItem.Quantity;
            UnitPrice = orderItem.UnitPrice;
            TotalPrice = orderItem.TotalPrice;
        }

        /// <summary>
        /// Product ID
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product SKU
        /// </summary>
        public string ProductSKU { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total price
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
