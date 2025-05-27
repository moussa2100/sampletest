using System;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Orders
{
    /// <summary>
    /// Service for updating orders
    /// </summary>
    [AutoRegister]
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the UpdateOrderService class
        /// </summary>
        /// <param name="orderRepository">Order repository</param>
        public UpdateOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        /// <summary>
        /// Updates order customer information
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="customerId">Customer ID (optional)</param>
        public void UpdateCustomer(Order order, string customerName, string customerEmail, Guid? customerId)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.SetCustomer(customerName, customerEmail, customerId);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Updates order shipping address
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="shippingAddress">New shipping address</param>
        public void UpdateShippingAddress(Order order, Address shippingAddress)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.SetShippingAddress(shippingAddress);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Updates order status
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="status">New order status</param>
        public void UpdateStatus(Order order, OrderStatus status)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.SetStatus(status);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Updates order notes
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="notes">Order notes</param>
        public void UpdateNotes(Order order, string notes)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.SetNotes(notes);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Adds an item to the order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="orderItem">The order item to add</param>
        public void AddItem(Order order, OrderItem orderItem)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.AddItem(orderItem);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Removes an item from the order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="productId">The product ID to remove</param>
        public void RemoveItem(Order order, Guid productId)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.RemoveItem(productId);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Updates the quantity of an existing item
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="productId">The product ID</param>
        /// <param name="newQuantity">The new quantity</param>
        public void UpdateItemQuantity(Order order, Guid productId, int newQuantity)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.UpdateItemQuantity(productId, newQuantity);
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Clears all items from the order
        /// </summary>
        /// <param name="order">The order to update</param>
        public void ClearItems(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.ClearItems();
            _orderRepository.Save(order);
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="order">The order to cancel</param>
        public void CancelOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.Cancel();
            _orderRepository.Save(order);
        }
    }
}
