using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Orders;
using Core.Services.Products;
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for Order management operations
    /// </summary>
    [RoutePrefix("api/orders")]
    public class OrderController : BaseApiController
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IDeleteOrderService _deleteOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IGetProductService _getProductService;

        /// <summary>
        /// Initializes a new instance of the OrderController class
        /// </summary>
        public OrderController(
            ICreateOrderService createOrderService,
            IDeleteOrderService deleteOrderService,
            IGetOrderService getOrderService,
            IUpdateOrderService updateOrderService,
            IGetProductService getProductService)
        {
            _createOrderService = createOrderService;
            _deleteOrderService = deleteOrderService;
            _getOrderService = getOrderService;
            _updateOrderService = updateOrderService;
            _getProductService = getProductService;
        }

        /// <summary>
        /// Creates a new order with auto-generated ID
        /// </summary>
        /// <param name="model">Order data</param>
        /// <returns>Created order</returns>
        [Route("")]
        [HttpPost]
        public HttpResponseMessage CreateOrder([FromBody] OrderModel model)
        {
            try
            {
                // Auto-generate a new GUID for the order
                var orderId = Guid.NewGuid();

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.CustomerName))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Customer name is required");
                }

                if (string.IsNullOrWhiteSpace(model.CustomerEmail))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Customer email is required");
                }

                if (model.ShippingAddress == null)
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Shipping address is required");
                }

                var shippingAddress = model.ShippingAddress.ToAddress();
                var order = _createOrderService.Create(
                    orderId,
                    model.CustomerName,
                    model.CustomerEmail,
                    model.CustomerId,
                    shippingAddress,
                    model.Notes);

                return Request.CreateResponse(System.Net.HttpStatusCode.Created, new OrderData(order));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new order with items and auto-generated ID
        /// </summary>
        /// <param name="model">Order data with items</param>
        /// <returns>Created order</returns>
        [Route("with-items")]
        [HttpPost]
        public HttpResponseMessage CreateOrderWithItems([FromBody] OrderModel model)
        {
            try
            {
                // Auto-generate a new GUID for the order
                var orderId = Guid.NewGuid();

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.CustomerName))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Customer name is required");
                }

                if (string.IsNullOrWhiteSpace(model.CustomerEmail))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Customer email is required");
                }

                if (model.ShippingAddress == null)
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Shipping address is required");
                }

                // Convert order items
                var orderItems = new System.Collections.Generic.List<OrderItem>();
                if (model.OrderItems != null)
                {
                    foreach (var itemModel in model.OrderItems)
                    {
                        var product = _getProductService.GetProduct(itemModel.ProductId);
                        if (product == null)
                        {
                            return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                                $"Product with ID {itemModel.ProductId} not found");
                        }

                        if (!product.IsAvailable(itemModel.Quantity))
                        {
                            return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                                $"Product {product.Name} is not available in the requested quantity");
                        }

                        var orderItem = OrderItem.FromProduct(product, itemModel.Quantity);
                        orderItems.Add(orderItem);
                    }
                }

                var shippingAddress = model.ShippingAddress.ToAddress();
                var order = _createOrderService.CreateWithItems(
                    orderId,
                    model.CustomerName,
                    model.CustomerEmail,
                    model.CustomerId,
                    shippingAddress,
                    orderItems,
                    model.Notes);

                return Request.CreateResponse(System.Net.HttpStatusCode.Created, new OrderData(order));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="model">Updated order data</param>
        /// <returns>Updated order</returns>
        [Route("{orderId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateOrder(Guid orderId, [FromBody] OrderModel model)
        {
            try
            {
                var order = _getOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return DoesNotExist();
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.CustomerName))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Customer name is required");
                }

                if (string.IsNullOrWhiteSpace(model.CustomerEmail))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Customer email is required");
                }

                if (model.ShippingAddress == null)
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Shipping address is required");
                }

                _updateOrderService.UpdateCustomer(order, model.CustomerName, model.CustomerEmail, model.CustomerId);
                _updateOrderService.UpdateShippingAddress(order, model.ShippingAddress.ToAddress());
                _updateOrderService.UpdateNotes(order, model.Notes);

                return Found(new OrderData(order));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets an order by ID
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Order data</returns>
        [Route("{orderId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetOrder(Guid orderId)
        {
            var order = _getOrderService.GetOrder(orderId);
            if (order == null)
            {
                return DoesNotExist();
            }

            return Found(new OrderData(order));
        }

        /// <summary>
        /// Gets an order by order number
        /// </summary>
        /// <param name="orderNumber">The order number</param>
        /// <returns>Order data</returns>
        [Route("number/{orderNumber}")]
        [HttpGet]
        public HttpResponseMessage GetOrderByNumber(string orderNumber)
        {
            var order = _getOrderService.GetOrderByNumber(orderNumber);
            if (order == null)
            {
                return DoesNotExist();
            }

            return Found(new OrderData(order));
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Success response</returns>
        [Route("{orderId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder(Guid orderId)
        {
            var order = _getOrderService.GetOrder(orderId);
            if (order == null)
            {
                return DoesNotExist();
            }

            _deleteOrderService.Delete(order);
            return Found();
        }

        /// <summary>
        /// Gets orders with filtering and pagination
        /// </summary>
        /// <param name="skip">Number of orders to skip</param>
        /// <param name="take">Number of orders to take</param>
        /// <param name="customerId">Customer ID filter</param>
        /// <param name="customerEmail">Customer email filter</param>
        /// <param name="status">Status filter</param>
        /// <param name="startDate">Start date filter</param>
        /// <param name="endDate">End date filter</param>
        /// <param name="minAmount">Minimum amount filter</param>
        /// <param name="maxAmount">Maximum amount filter</param>
        /// <param name="productId">Product ID filter</param>
        /// <returns>Filtered orders</returns>
        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetOrders(
            int skip = 0,
            int take = 50,
            Guid? customerId = null,
            string customerEmail = null,
            OrderStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            Guid? productId = null)
        {
            try
            {
                var orders = _getOrderService.GetOrders(customerId, customerEmail, status, startDate, endDate, minAmount, maxAmount, productId)
                                            .Skip(skip)
                                            .Take(take)
                                            .Select(o => new OrderData(o))
                                            .ToList();

                return Found(orders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets orders by customer ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Customer orders</returns>
        [Route("customer/{customerId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetOrdersByCustomer(Guid customerId)
        {
            try
            {
                var orders = _getOrderService.GetOrdersByCustomerId(customerId)
                                            .Select(o => new OrderData(o))
                                            .ToList();

                return Found(orders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets orders by status
        /// </summary>
        /// <param name="statusId">The status ID</param>
        /// <returns>Orders with the specified status</returns>
        [Route("status/{statusId:int}")]
        [HttpGet]
        public HttpResponseMessage GetOrdersByStatus(int statusId)
        {
            try
            {
                if (!Enum.IsDefined(typeof(OrderStatus), statusId))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Invalid status ID");
                }

                var status = (OrderStatus)statusId;
                var orders = _getOrderService.GetOrdersByStatus(status)
                                            .Select(o => new OrderData(o))
                                            .ToList();

                return Found(orders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates order status
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="model">Status update data</param>
        /// <returns>Updated order</returns>
        [Route("{orderId:guid}/status")]
        [HttpPost]
        public HttpResponseMessage UpdateOrderStatus(Guid orderId, [FromBody] OrderStatusUpdateModel model)
        {
            try
            {
                var order = _getOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return DoesNotExist();
                }

                _updateOrderService.UpdateStatus(order, model.Status);
                return Found(new OrderData(order));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Adds an item to an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="model">Order item data</param>
        /// <returns>Updated order</returns>
        [Route("{orderId:guid}/items/add")]
        [HttpPost]
        public HttpResponseMessage AddItemToOrder(Guid orderId, [FromBody] OrderItemModel model)
        {
            try
            {
                var order = _getOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return DoesNotExist();
                }

                var product = _getProductService.GetProduct(model.ProductId);
                if (product == null)
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        $"Product with ID {model.ProductId} not found");
                }

                if (!product.IsAvailable(model.Quantity))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        $"Product {product.Name} is not available in the requested quantity");
                }

                var orderItem = OrderItem.FromProduct(product, model.Quantity);
                _updateOrderService.AddItem(order, orderItem);

                return Found(new OrderData(order));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Removes an item from an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="productId">The product ID to remove</param>
        /// <returns>Updated order</returns>
        [Route("{orderId:guid}/items/{productId:guid}")]
        [HttpDelete]
        public HttpResponseMessage RemoveItemFromOrder(Guid orderId, Guid productId)
        {
            try
            {
                var order = _getOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return DoesNotExist();
                }

                _updateOrderService.RemoveItem(order, productId);
                return Found(new OrderData(order));
            }
            catch (InvalidOperationException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the quantity of an item in an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="productId">The product ID</param>
        /// <param name="model">Quantity update data</param>
        /// <returns>Updated order</returns>
        [Route("{orderId:guid}/items/{productId:guid}/quantity")]
        [HttpPost]
        public HttpResponseMessage UpdateItemQuantity(Guid orderId, Guid productId, [FromBody] OrderItemModel model)
        {
            try
            {
                var order = _getOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return DoesNotExist();
                }

                _updateOrderService.UpdateItemQuantity(order, productId, model.Quantity);
                return Found(new OrderData(order));
            }
            catch (ArgumentException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>Updated order</returns>
        [Route("{orderId:guid}/cancel")]
        [HttpPost]
        public HttpResponseMessage CancelOrder(Guid orderId)
        {
            try
            {
                var order = _getOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return DoesNotExist();
                }

                _updateOrderService.CancelOrder(order);
                return Found(new OrderData(order));
            }
            catch (InvalidOperationException ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets order statistics for a date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Order statistics</returns>
        [Route("statistics")]
        [HttpGet]
        public HttpResponseMessage GetOrderStatistics(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;

                var statistics = _getOrderService.GetStatistics(start, end);
                return Found(statistics);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Clears all orders (for testing purposes)
        /// </summary>
        /// <returns>Success response</returns>
        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage ClearAllOrders()
        {
            _deleteOrderService.DeleteAll();
            return Found();
        }
    }
}