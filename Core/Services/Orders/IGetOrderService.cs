using System;
using System.Collections.Generic;
using BusinessEntities;
using Data.Repositories;

namespace Core.Services.Orders
{
    /// <summary>
    /// Interface for retrieving orders
    /// </summary>
    public interface IGetOrderService
    {
        /// <summary>
        /// Gets an order by ID
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <returns>The order if found, null otherwise</returns>
        Order GetOrder(Guid id);

        /// <summary>
        /// Gets an order by order number
        /// </summary>
        /// <param name="orderNumber">The order number</param>
        /// <returns>The order if found, null otherwise</returns>
        Order GetOrderByNumber(string orderNumber);

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>All orders</returns>
        IEnumerable<Order> GetOrders();

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
        IEnumerable<Order> GetOrders(
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null);

        /// <summary>
        /// Gets orders by customer ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Orders for the specified customer</returns>
        IEnumerable<Order> GetOrdersByCustomerId(Guid customerId);

        /// <summary>
        /// Gets orders by customer email
        /// </summary>
        /// <param name="customerEmail">The customer email</param>
        /// <returns>Orders for the specified customer email</returns>
        IEnumerable<Order> GetOrdersByCustomerEmail(string customerEmail);

        /// <summary>
        /// Gets orders by status
        /// </summary>
        /// <param name="status">The order status</param>
        /// <returns>Orders with the specified status</returns>
        IEnumerable<Order> GetOrdersByStatus(OrderStatus status);

        /// <summary>
        /// Gets orders containing a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Orders containing the specified product</returns>
        IEnumerable<Order> GetOrdersByProduct(Guid productId);

        /// <summary>
        /// Gets recent orders
        /// </summary>
        /// <param name="days">Number of days to look back</param>
        /// <returns>Recent orders</returns>
        IEnumerable<Order> GetRecentOrders(int days = 30);

        /// <summary>
        /// Gets order statistics for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Order statistics</returns>
        OrderStatistics GetStatistics(DateTime startDate, DateTime endDate);
    }
}
