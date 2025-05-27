namespace BusinessEntities
{
    /// <summary>
    /// Represents the status of an order throughout its lifecycle
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order has been created but not yet processed
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Order is being processed and prepared
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Order has been shipped to the customer
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// Order has been delivered to the customer
        /// </summary>
        Delivered = 4,

        /// <summary>
        /// Order has been cancelled
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Order has been returned by the customer
        /// </summary>
        Returned = 6,

        /// <summary>
        /// Order has been refunded
        /// </summary>
        Refunded = 7
    }
}
