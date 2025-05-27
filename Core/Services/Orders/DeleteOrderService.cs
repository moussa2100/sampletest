using System;
using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Orders
{
    /// <summary>
    /// Service for deleting orders
    /// </summary>
    [AutoRegister]
    public class DeleteOrderService : IDeleteOrderService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteOrderService class
        /// </summary>
        /// <param name="orderRepository">Order repository</param>
        public DeleteOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order to delete</param>
        public void Delete(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _orderRepository.Delete(order);
        }

        /// <summary>
        /// Deletes all orders (for testing/cleanup purposes)
        /// </summary>
        public void DeleteAll()
        {
            _orderRepository.Clear();
        }
    }
}
