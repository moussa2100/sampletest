using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;

namespace Core.Services.Orders
{
    /// <summary>
    /// Service for creating orders
    /// </summary>
    [AutoRegister]
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IIdObjectFactory<Order> _orderFactory;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the CreateOrderService class
        /// </summary>
        /// <param name="orderFactory">Order factory</param>
        /// <param name="orderRepository">Order repository</param>
        /// <param name="updateOrderService">Update order service</param>
        public CreateOrderService(IIdObjectFactory<Order> orderFactory, IOrderRepository orderRepository, IUpdateOrderService updateOrderService)
        {
            _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _updateOrderService = updateOrderService ?? throw new ArgumentNullException(nameof(updateOrderService));
        }

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
        public Order Create(Guid id, string customerName, string customerEmail, Guid? customerId, Address shippingAddress, string notes = null)
        {
            var order = _orderFactory.Create(id);
            order.SetCustomer(customerName, customerEmail, customerId);
            order.SetShippingAddress(shippingAddress);
            
            if (!string.IsNullOrWhiteSpace(notes))
            {
                order.SetNotes(notes);
            }

            _orderRepository.Save(order);
            return order;
        }

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
        public Order CreateWithItems(Guid id, string customerName, string customerEmail, Guid? customerId, Address shippingAddress, IEnumerable<OrderItem> orderItems, string notes = null)
        {
            var order = Create(id, customerName, customerEmail, customerId, shippingAddress, notes);

            if (orderItems != null)
            {
                foreach (var item in orderItems)
                {
                    order.AddItem(item);
                }
                _orderRepository.Save(order);
            }

            return order;
        }
    }
}
