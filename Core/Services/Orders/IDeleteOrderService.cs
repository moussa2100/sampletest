using BusinessEntities;

namespace Core.Services.Orders
{
    /// <summary>
    /// Interface for deleting orders
    /// </summary>
    public interface IDeleteOrderService
    {
        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order to delete</param>
        void Delete(Order order);

        /// <summary>
        /// Deletes all orders (for testing/cleanup purposes)
        /// </summary>
        void DeleteAll();
    }
}
