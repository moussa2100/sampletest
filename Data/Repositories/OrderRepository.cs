using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;

namespace Data.Repositories
{
    /// <summary>
    /// Order repository implementation with specialized order operations
    /// </summary>
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderRepository : InMemoryRepository<Order>, IOrderRepository
    {
        /// <summary>
        /// Gets orders by customer ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Orders for the specified customer</returns>
        public IEnumerable<Order> GetByCustomerId(Guid customerId)
        {
            return Find(o => o.CustomerId == customerId);
        }

        /// <summary>
        /// Gets orders by customer email
        /// </summary>
        /// <param name="customerEmail">The customer email</param>
        /// <returns>Orders for the specified customer email</returns>
        public IEnumerable<Order> GetByCustomerEmail(string customerEmail)
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                return Enumerable.Empty<Order>();
            }

            var searchEmail = customerEmail.Trim().ToLowerInvariant();
            return Find(o => o.CustomerEmail != null && o.CustomerEmail.ToLowerInvariant() == searchEmail);
        }

        /// <summary>
        /// Gets orders by status
        /// </summary>
        /// <param name="status">The order status</param>
        /// <returns>Orders with the specified status</returns>
        public IEnumerable<Order> GetByStatus(OrderStatus status)
        {
            return Find(o => o.Status == status);
        }

        /// <summary>
        /// Gets orders by order number (exact match)
        /// </summary>
        /// <param name="orderNumber">The order number</param>
        /// <returns>Order with the specified order number, or null if not found</returns>
        public Order GetByOrderNumber(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return null;
            }

            var searchOrderNumber = orderNumber.Trim().ToUpperInvariant();
            return Find(o => o.OrderNumber != null && o.OrderNumber.ToUpperInvariant() == searchOrderNumber).FirstOrDefault();
        }

        /// <summary>
        /// Gets orders within a date range
        /// </summary>
        /// <param name="startDate">Start date (inclusive)</param>
        /// <param name="endDate">End date (inclusive)</param>
        /// <returns>Orders within the date range</returns>
        public IEnumerable<Order> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.", nameof(endDate));
            }

            return Find(o => o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date);
        }

        /// <summary>
        /// Gets orders within a total amount range
        /// </summary>
        /// <param name="minAmount">Minimum amount (inclusive)</param>
        /// <param name="maxAmount">Maximum amount (inclusive)</param>
        /// <returns>Orders within the amount range</returns>
        public IEnumerable<Order> GetByAmountRange(decimal minAmount, decimal maxAmount)
        {
            if (minAmount < 0)
            {
                throw new ArgumentException("Minimum amount cannot be negative.", nameof(minAmount));
            }

            if (maxAmount < minAmount)
            {
                throw new ArgumentException("Maximum amount cannot be less than minimum amount.", nameof(maxAmount));
            }

            return Find(o => o.TotalAmount >= minAmount && o.TotalAmount <= maxAmount);
        }

        /// <summary>
        /// Gets orders containing a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Orders containing the specified product</returns>
        public IEnumerable<Order> GetByProductId(Guid productId)
        {
            return Find(o => o.OrderItems.Any(item => item.ProductId == productId));
        }

        /// <summary>
        /// Gets recent orders (within specified days)
        /// </summary>
        /// <param name="days">Number of days to look back</param>
        /// <returns>Recent orders</returns>
        public IEnumerable<Order> GetRecentOrders(int days = 30)
        {
            if (days < 0)
            {
                throw new ArgumentException("Days cannot be negative.", nameof(days));
            }

            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return Find(o => o.OrderDate >= cutoffDate);
        }

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
        public IEnumerable<Order> GetFiltered(
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null,
            int skip = 0,
            int take = 50)
        {
            if (skip < 0)
            {
                throw new ArgumentException("Skip cannot be negative.", nameof(skip));
            }

            if (take <= 0)
            {
                throw new ArgumentException("Take must be greater than zero.", nameof(take));
            }

            var query = GetAll().AsQueryable();

            // Apply filters
            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                var searchEmail = customerEmail.Trim().ToLowerInvariant();
                query = query.Where(o => o.CustomerEmail != null && o.CustomerEmail.ToLowerInvariant() == searchEmail);
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date <= endDate.Value.Date);
            }

            if (minAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= maxAmount.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(o => o.OrderItems.Any(item => item.ProductId == productId.Value));
            }

            // Apply pagination and ordering (most recent first)
            return query.OrderByDescending(o => o.OrderDate).Skip(skip).Take(take).ToList();
        }

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
        public int GetFilteredCount(
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null)
        {
            var query = GetAll().AsQueryable();

            // Apply filters (same logic as GetFiltered)
            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                var searchEmail = customerEmail.Trim().ToLowerInvariant();
                query = query.Where(o => o.CustomerEmail != null && o.CustomerEmail.ToLowerInvariant() == searchEmail);
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date <= endDate.Value.Date);
            }

            if (minAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= maxAmount.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(o => o.OrderItems.Any(item => item.ProductId == productId.Value));
            }

            return query.Count();
        }

        /// <summary>
        /// Checks if an order number already exists
        /// </summary>
        /// <param name="orderNumber">The order number to check</param>
        /// <param name="excludeOrderId">Order ID to exclude from the check (for updates)</param>
        /// <returns>True if order number exists, false otherwise</returns>
        public bool OrderNumberExists(string orderNumber, Guid? excludeOrderId = null)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return false;
            }

            var searchOrderNumber = orderNumber.Trim().ToUpperInvariant();

            if (excludeOrderId.HasValue)
            {
                return Any(o => o.OrderNumber != null &&
                               o.OrderNumber.ToUpperInvariant() == searchOrderNumber &&
                               o.Id != excludeOrderId.Value);
            }

            return Any(o => o.OrderNumber != null && o.OrderNumber.ToUpperInvariant() == searchOrderNumber);
        }

        /// <summary>
        /// Gets order statistics for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Order statistics</returns>
        public OrderStatistics GetStatistics(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.", nameof(endDate));
            }

            var ordersInRange = GetByDateRange(startDate, endDate).ToList();

            var statistics = new OrderStatistics
            {
                TotalOrders = ordersInRange.Count,
                TotalRevenue = ordersInRange.Sum(o => o.TotalAmount),
                PendingOrders = ordersInRange.Count(o => o.Status == OrderStatus.Pending),
                CompletedOrders = ordersInRange.Count(o => o.Status == OrderStatus.Delivered),
                CancelledOrders = ordersInRange.Count(o => o.Status == OrderStatus.Cancelled)
            };

            statistics.AverageOrderValue = statistics.TotalOrders > 0
                ? statistics.TotalRevenue / statistics.TotalOrders
                : 0;

            return statistics;
        }
    }
}
