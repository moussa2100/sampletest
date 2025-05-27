using BusinessEntities;

namespace Core.Services.Products
{
    /// <summary>
    /// Interface for deleting products
    /// </summary>
    public interface IDeleteProductService
    {
        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="product">The product to delete</param>
        void Delete(Product product);

        /// <summary>
        /// Deletes all products (for testing/cleanup purposes)
        /// </summary>
        void DeleteAll();
    }
}
