namespace WebApi.Models.Products
{
    /// <summary>
    /// Model for updating product stock
    /// </summary>
    public class StockUpdateModel
    {
        /// <summary>
        /// Quantity to set, add, or remove
        /// </summary>
        public int Quantity { get; set; }
    }
}
