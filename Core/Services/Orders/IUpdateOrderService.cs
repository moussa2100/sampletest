using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Orders
{
    /// <summary>
    /// Interface for updating orders
    /// </summary>
    public interface IUpdateOrderService
    {
        /// <summary>
        /// Updates order customer information
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="customerId">Customer ID (optional)</param>
        void UpdateCustomer(Order order, string customerName, string customerEmail, Guid? customerId);

        /// <summary>
        /// Updates order shipping address
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="shippingAddress">New shipping address</param>
        void UpdateShippingAddress(Order order, Address shippingAddress);

        /// <summary>
        /// Updates order status
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="status">New order status</param>
        void UpdateStatus(Order order, OrderStatus status);

        /// <summary>
        /// Updates order notes
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="notes">Order notes</param>
        void UpdateNotes(Order order, string notes);

        /// <summary>
        /// Adds an item to the order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="orderItem">The order item to add</param>
        void AddItem(Order order, OrderItem orderItem);

        /// <summary>
        /// Removes an item from the order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="productId">The product ID to remove</param>
        void RemoveItem(Order order, Guid productId);

        /// <summary>
        /// Updates the quantity of an existing item
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="productId">The product ID</param>
        /// <param name="newQuantity">The new quantity</param>
        void UpdateItemQuantity(Order order, Guid productId, int newQuantity);

        /// <summary>
        /// Clears all items from the order
        /// </summary>
        /// <param name="order">The order to update</param>
        void ClearItems(Order order);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="order">The order to cancel</param>
        void CancelOrder(Order order);
    }
}
