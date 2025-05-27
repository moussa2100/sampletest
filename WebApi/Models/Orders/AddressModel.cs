using BusinessEntities;

namespace WebApi.Models.Orders
{
    /// <summary>
    /// Model for address information
    /// </summary>
    public class AddressModel
    {
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

        /// <summary>
        /// Converts to Address entity
        /// </summary>
        /// <returns>Address entity</returns>
        public Address ToAddress()
        {
            var address = new Address();
            address.SetStreet(Street);
            address.SetCity(City);
            address.SetState(State);
            address.SetPostalCode(PostalCode);
            address.SetCountry(Country);
            return address;
        }
    }
}
