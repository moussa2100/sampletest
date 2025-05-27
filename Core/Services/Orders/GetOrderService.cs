using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Orders
{
    /// <summary>
    /// Service for retrieving orders
    /// </summary>
    [AutoRegister]
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the GetOrderService class
        /// </summary>
        /// <param name="orderRepository">Order repository</param>
        public GetOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        /// <summary>
        /// Gets an order by ID
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <returns>The order if found, null otherwise</returns>
        public Order GetOrder(Guid id)
        {
            return _orderRepository.Get(id);
        }

        /// <summary>
        /// Gets an order by order number
        /// </summary>
        /// <param name="orderNumber">The order number</param>
        /// <returns>The order if found, null otherwise</returns>
        public Order GetOrderByNumber(string orderNumber)
        {
            return _orderRepository.GetByOrderNumber(orderNumber);
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>All orders</returns>
        public IEnumerable<Order> GetOrders()
        {
            return _orderRepository.GetAll();
        }

        /// <summary>
        /// Gets orders with filtering
        /// </summary>
        /// <param name="customerId">Customer ID filter (optional)</param>
        /// <param name="customerEmail">Customer email filter (optional)</param>
        /// <param name="status">Status filter (optional)</param>
        /// <param name="startDate">Start date filter (optional)</param>
        /// <param name="endDate">End date filter (optional)</param>
        /// <param name="minAmount">Minimum amount filter (optional)</param>
        /// <param name="maxAmount">Maximum amount filter (optional)</param>
        /// <param name="productId">Product ID filter (optional)</param>
        /// <returns>Filtered orders</returns>
        public IEnumerable<Order> GetOrders(
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null)
        {
            return _orderRepository.GetFiltered(customerId, customerEmail, status, startDate, endDate, minAmount, maxAmount, productId);
        }

        /// <summary>
        /// Gets orders by customer ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Orders for the specified customer</returns>
        public IEnumerable<Order> GetOrdersByCustomerId(Guid customerId)
        {
            return _orderRepository.GetByCustomerId(customerId);
        }

        /// <summary>
        /// Gets orders by customer email
        /// </summary>
        /// <param name="customerEmail">The customer email</param>
        /// <returns>Orders for the specified customer email</returns>
        public IEnumerable<Order> GetOrdersByCustomerEmail(string customerEmail)
        {
            return _orderRepository.GetByCustomerEmail(customerEmail);
        }

        /// <summary>
        /// Gets orders by status
        /// </summary>
        /// <param name="status">The order status</param>
        /// <returns>Orders with the specified status</returns>
        public IEnumerable<Order> GetOrdersByStatus(OrderStatus status)
        {
            return _orderRepository.GetByStatus(status);
        }

        /// <summary>
        /// Gets orders containing a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Orders containing the specified product</returns>
        public IEnumerable<Order> GetOrdersByProduct(Guid productId)
        {
            return _orderRepository.GetByProductId(productId);
        }

        /// <summary>
        /// Gets recent orders
        /// </summary>
        /// <param name="days">Number of days to look back</param>
        /// <returns>Recent orders</returns>
        public IEnumerable<Order> GetRecentOrders(int days = 30)
        {
            return _orderRepository.GetRecentOrders(days);
        }

        /// <summary>
        /// Gets order statistics for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Order statistics</returns>
        public OrderStatistics GetStatistics(DateTime startDate, DateTime endDate)
        {
            return _orderRepository.GetStatistics(startDate, endDate);
        }
    }
}
