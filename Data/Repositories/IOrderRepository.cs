using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Data.Repositories
{
    /// <summary>
    /// Interface for order-specific repository operations
    /// </summary>
    public interface IOrderRepository : IInMemoryRepository<Order>
    {
        /// <summary>
        /// Gets orders by customer ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Orders for the specified customer</returns>
        IEnumerable<Order> GetByCustomerId(Guid customerId);

        /// <summary>
        /// Gets orders by customer email
        /// </summary>
        /// <param name="customerEmail">The customer email</param>
        /// <returns>Orders for the specified customer email</returns>
        IEnumerable<Order> GetByCustomerEmail(string customerEmail);

        /// <summary>
        /// Gets orders by status
        /// </summary>
        /// <param name="status">The order status</param>
        /// <returns>Orders with the specified status</returns>
        IEnumerable<Order> GetByStatus(OrderStatus status);

        /// <summary>
        /// Gets orders by order number (exact match)
        /// </summary>
        /// <param name="orderNumber">The order number</param>
        /// <returns>Order with the specified order number, or null if not found</returns>
        Order GetByOrderNumber(string orderNumber);

        /// <summary>
        /// Gets orders within a date range
        /// </summary>
        /// <param name="startDate">Start date (inclusive)</param>
        /// <param name="endDate">End date (inclusive)</param>
        /// <returns>Orders within the date range</returns>
        IEnumerable<Order> GetByDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets orders within a total amount range
        /// </summary>
        /// <param name="minAmount">Minimum amount (inclusive)</param>
        /// <param name="maxAmount">Maximum amount (inclusive)</param>
        /// <returns>Orders within the amount range</returns>
        IEnumerable<Order> GetByAmountRange(decimal minAmount, decimal maxAmount);

        /// <summary>
        /// Gets orders containing a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Orders containing the specified product</returns>
        IEnumerable<Order> GetByProductId(Guid productId);

        /// <summary>
        /// Gets recent orders (within specified days)
        /// </summary>
        /// <param name="days">Number of days to look back</param>
        /// <returns>Recent orders</returns>
        IEnumerable<Order> GetRecentOrders(int days = 30);

        /// <summary>
        /// Gets orders with advanced filtering
        /// </summary>
        /// <param name="customerId">Customer ID filter (optional)</param>
        /// <param name="customerEmail">Customer email filter (optional)</param>
        /// <param name="status">Status filter (optional)</param>
        /// <param name="startDate">Start date filter (optional)</param>
        /// <param name="endDate">End date filter (optional)</param>
        /// <param name="minAmount">Minimum amount filter (optional)</param>
        /// <param name="maxAmount">Maximum amount filter (optional)</param>
        /// <param name="productId">Product ID filter (optional)</param>
        /// <param name="skip">Number of orders to skip for pagination</param>
        /// <param name="take">Number of orders to take for pagination</param>
        /// <returns>Filtered and paginated orders</returns>
        IEnumerable<Order> GetFiltered(
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null,
            int skip = 0,
            int take = 50);

        /// <summary>
        /// Gets the count of orders matching the filter criteria
        /// </summary>
        /// <param name="customerId">Customer ID filter (optional)</param>
        /// <param name="customerEmail">Customer email filter (optional)</param>
        /// <param name="status">Status filter (optional)</param>
        /// <param name="startDate">Start date filter (optional)</param>
        /// <param name="endDate">End date filter (optional)</param>
        /// <param name="minAmount">Minimum amount filter (optional)</param>
        /// <param name="maxAmount">Maximum amount filter (optional)</param>
        /// <param name="productId">Product ID filter (optional)</param>
        /// <returns>Count of matching orders</returns>
        int GetFilteredCount(
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null);

        /// <summary>
        /// Checks if an order number already exists
        /// </summary>
        /// <param name="orderNumber">The order number to check</param>
        /// <param name="excludeOrderId">Order ID to exclude from the check (for updates)</param>
        /// <returns>True if order number exists, false otherwise</returns>
        bool OrderNumberExists(string orderNumber, Guid? excludeOrderId = null);

        /// <summary>
        /// Gets order statistics for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Order statistics</returns>
        OrderStatistics GetStatistics(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Represents order statistics for a given period
    /// </summary>
    public class OrderStatistics
    {
        /// <summary>
        /// Total number of orders
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Total revenue amount
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Average order value
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// Number of pending orders
        /// </summary>
        public int PendingOrders { get; set; }

        /// <summary>
        /// Number of completed orders
        /// </summary>
        public int CompletedOrders { get; set; }

        /// <summary>
        /// Number of cancelled orders
        /// </summary>
        public int CancelledOrders { get; set; }
    }
}
