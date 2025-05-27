using BusinessEntities;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Data transfer object for address responses
    /// </summary>
    public class AddressData
    {
        /// <summary>
        /// Initializes a new instance of AddressData
        /// </summary>
        /// <param name="address">The address entity</param>
        public AddressData(Address address)
        {
            Street = address.Street;
            City = address.City;
            State = address.State;
            PostalCode = address.PostalCode;
            Country = address.Country;
        }

        /// <summary>
        /// Street address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State or province
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Postal or ZIP code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }
}
