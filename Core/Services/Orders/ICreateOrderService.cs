using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Orders
{
    /// <summary>
    /// Interface for creating orders
    /// </summary>
    public interface ICreateOrderService
    {
        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="customerId">Customer ID (optional)</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <param name="notes">Order notes (optional)</param>
        /// <returns>The created order</returns>
        Order Create(Guid id, string customerName, string customerEmail, Guid? customerId, Address shippingAddress, string notes = null);

        /// <summary>
        /// Creates a new order with items
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="customerId">Customer ID (optional)</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <param name="orderItems">Order items</param>
        /// <param name="notes">Order notes (optional)</param>
        /// <returns>The created order</returns>
        Order CreateWithItems(Guid id, string customerName, string customerEmail, Guid? customerId, Address shippingAddress, IEnumerable<OrderItem> orderItems, string notes = null);
    }
}
