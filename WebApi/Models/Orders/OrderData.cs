using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Data transfer object for order responses
    /// </summary>
    public class OrderData : IdObjectData
    {
        /// <summary>
        /// Initializes a new instance of OrderData
        /// </summary>
        /// <param name="order">The order entity</param>
        public OrderData(Order order) : base(order)
        {
            OrderNumber = order.OrderNumber;
            CustomerId = order.CustomerId;
            CustomerName = order.CustomerName;
            CustomerEmail = order.CustomerEmail;
            OrderDate = order.OrderDate;
            Status = new EnumData(order.Status);
            TotalAmount = order.TotalAmount;
            OrderItems = order.OrderItems.Select(item => new OrderItemData(item)).ToList();
            ShippingAddress = order.ShippingAddress != null ? new AddressData(order.ShippingAddress) : null;
            CreatedDate = order.CreatedDate;
            UpdatedDate = order.UpdatedDate;
            Notes = order.Notes;
        }

        /// <summary>
        /// Order number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Customer ID
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Customer email
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Order date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        public EnumData Status { get; set; }

        /// <summary>
        /// Total amount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Order items
        /// </summary>
        public IEnumerable<OrderItemData> OrderItems { get; set; }

        /// <summary>
        /// Shipping address
        /// </summary>
        public AddressData ShippingAddress { get; set; }

        /// <summary>
        /// Date when the order was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the order was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Order notes
        /// </summary>
        public string Notes { get; set; }
    }
}
