using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Products;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for Product management operations
    /// </summary>
    [RoutePrefix("api/products")]
    public class ProductController : BaseApiController
    {
        private readonly ICreateProductService _createProductService;
        private readonly IDeleteProductService _deleteProductService;
        private readonly IGetProductService _getProductService;
        private readonly IUpdateProductService _updateProductService;

        /// <summary>
        /// Initializes a new instance of the ProductController class
        /// </summary>
        public ProductController(
            ICreateProductService createProductService,
            IDeleteProductService deleteProductService,
            IGetProductService getProductService,
            IUpdateProductService updateProductService)
        {
            _createProductService = createProductService;
            _deleteProductService = deleteProductService;
            _getProductService = getProductService;
            _updateProductService = updateProductService;
        }

        /// <summary>
        /// Creates a new product with auto-generated ID
        /// </summary>
        /// <param name="model">Product data</param>
        /// <returns>Created product</returns>
        [Route("")]
        [HttpPost]
        public HttpResponseMessage CreateProduct([FromBody] ProductModel model)
        {
            try
            {
                // Auto-generate a new GUID for the product
                var productId = Guid.NewGuid();

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Product name is required");
                }

                if (string.IsNullOrWhiteSpace(model.SKU))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Product SKU is required");
                }

                var product = _createProductService.Create(
                    productId,
                    model.Name,
                    model.Description,
                    model.Price,
                    model.Category,
                    model.SKU,
                    model.StockQuantity,
                    model.IsActive);

                return Request.CreateResponse(System.Net.HttpStatusCode.Created, new ProductData(product));
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
        /// Updates an existing product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="model">Updated product data</param>
        /// <returns>Updated product</returns>
        [Route("{productId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(Guid productId, [FromBody] ProductModel model)
        {
            try
            {
                var product = _getProductService.GetProduct(productId);
                if (product == null)
                {
                    return DoesNotExist();
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Product name is required");
                }

                if (string.IsNullOrWhiteSpace(model.SKU))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Product SKU is required");
                }

                _updateProductService.Update(
                    product,
                    model.Name,
                    model.Description,
                    model.Price,
                    model.Category,
                    model.SKU,
                    model.StockQuantity,
                    model.IsActive);

                return Found(new ProductData(product));
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
        /// Gets a product by ID
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Product data</returns>
        [Route("{productId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);
            if (product == null)
            {
                return DoesNotExist();
            }

            return Found(new ProductData(product));
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <returns>Success response</returns>
        [Route("{productId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);
            if (product == null)
            {
                return DoesNotExist();
            }

            _deleteProductService.Delete(product);
            return Found();
        }

        /// <summary>
        /// Gets products with filtering and pagination
        /// </summary>
        /// <param name="skip">Number of products to skip</param>
        /// <param name="take">Number of products to take</param>
        /// <param name="name">Name filter</param>
        /// <param name="category">Category filter</param>
        /// <param name="minPrice">Minimum price filter</param>
        /// <param name="maxPrice">Maximum price filter</param>
        /// <param name="isActive">Active status filter</param>
        /// <returns>Filtered products</returns>
        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetProducts(
            int skip = 0,
            int take = 50,
            string name = null,
            ProductCategory? category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isActive = null)
        {
            try
            {
                var products = _getProductService.GetProducts(name, category, minPrice, maxPrice, isActive)
                                                 .Skip(skip)
                                                 .Take(take)
                                                 .Select(p => new ProductData(p))
                                                 .ToList();

                return Found(products);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets products by category
        /// </summary>
        /// <param name="categoryId">The category ID</param>
        /// <returns>Products in the category</returns>
        [Route("category/{categoryId:int}")]
        [HttpGet]
        public HttpResponseMessage GetProductsByCategory(int categoryId)
        {
            try
            {
                if (!Enum.IsDefined(typeof(ProductCategory), categoryId))
                {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,
                        "Invalid category ID");
                }

                var category = (ProductCategory)categoryId;
                var products = _getProductService.GetProductsByCategory(category)
                                                 .Select(p => new ProductData(p))
                                                 .ToList();

                return Found(products);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a product by SKU
        /// </summary>
        /// <param name="sku">The product SKU</param>
        /// <returns>Product data</returns>
        [Route("sku/{sku}")]
        [HttpGet]
        public HttpResponseMessage GetProductBySKU(string sku)
        {
            var product = _getProductService.GetProductBySKU(sku);
            if (product == null)
            {
                return DoesNotExist();
            }

            return Found(new ProductData(product));
        }

        /// <summary>
        /// Gets products with low stock
        /// </summary>
        /// <param name="threshold">Stock threshold</param>
        /// <returns>Low stock products</returns>
        [Route("lowstock")]
        [HttpGet]
        public HttpResponseMessage GetLowStockProducts(int threshold = 10)
        {
            try
            {
                var products = _getProductService.GetLowStockProducts(threshold)
                                                 .Select(p => new ProductData(p))
                                                 .ToList();

                return Found(products);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates product stock quantity
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="model">Stock update data</param>
        /// <returns>Updated product</returns>
        [Route("{productId:guid}/stock/update")]
        [HttpPost]
        public HttpResponseMessage UpdateStock(Guid productId, [FromBody] StockUpdateModel model)
        {
            try
            {
                var product = _getProductService.GetProduct(productId);
                if (product == null)
                {
                    return DoesNotExist();
                }

                _updateProductService.UpdateStock(product, model.Quantity);
                return Found(new ProductData(product));
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
        /// Adds stock to a product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="model">Stock update data</param>
        /// <returns>Updated product</returns>
        [Route("{productId:guid}/stock/add")]
        [HttpPost]
        public HttpResponseMessage AddStock(Guid productId, [FromBody] StockUpdateModel model)
        {
            try
            {
                var product = _getProductService.GetProduct(productId);
                if (product == null)
                {
                    return DoesNotExist();
                }

                _updateProductService.AddStock(product, model.Quantity);
                return Found(new ProductData(product));
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
        /// Removes stock from a product
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="model">Stock update data</param>
        /// <returns>Updated product</returns>
        [Route("{productId:guid}/stock/remove")]
        [HttpPost]
        public HttpResponseMessage RemoveStock(Guid productId, [FromBody] StockUpdateModel model)
        {
            try
            {
                var product = _getProductService.GetProduct(productId);
                if (product == null)
                {
                    return DoesNotExist();
                }

                _updateProductService.RemoveStock(product, model.Quantity);
                return Found(new ProductData(product));
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
        /// Clears all products (for testing purposes)
        /// </summary>
        /// <returns>Success response</returns>
        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage ClearAllProducts()
        {
            _deleteProductService.DeleteAll();
            return Found();
        }
    }
}