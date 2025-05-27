using BusinessEntities;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Model for updating order status
    /// </summary>
    public class OrderStatusUpdateModel
    {
        /// <summary>
        /// New order status
        /// </summary>
        public OrderStatus Status { get; set; }
    }
}
