using System;

namespace BusinessEntities
{
    /// <summary>
    /// Represents a shipping or billing address
    /// </summary>
    public class Address
    {
        private string _street;
        private string _city;
        private string _state;
        private string _postalCode;
        private string _country;

        /// <summary>
        /// Initializes a new instance of the Address class
        /// </summary>
        public Address()
        {
        }

        /// <summary>
        /// Gets the street address
        /// </summary>
        public string Street
        {
            get => _street;
            private set => _street = value;
        }

        /// <summary>
        /// Gets the city
        /// </summary>
        public string City
        {
            get => _city;
            private set => _city = value;
        }

        /// <summary>
        /// Gets the state or province
        /// </summary>
        public string State
        {
            get => _state;
            private set => _state = value;
        }

        /// <summary>
        /// Gets the postal or ZIP code
        /// </summary>
        public string PostalCode
        {
            get => _postalCode;
            private set => _postalCode = value;
        }

        /// <summary>
        /// Gets the country
        /// </summary>
        public string Country
        {
            get => _country;
            private set => _country = value;
        }

        /// <summary>
        /// Sets the street address with validation
        /// </summary>
        /// <param name="street">The street address</param>
        /// <exception cref="ArgumentException">Thrown when street is invalid</exception>
        public void SetStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentException("Street address cannot be null or empty.", nameof(street));
            }

            if (street.Length > 200)
            {
                throw new ArgumentException("Street address cannot exceed 200 characters.", nameof(street));
            }

            _street = street.Trim();
        }

        /// <summary>
        /// Sets the city with validation
        /// </summary>
        /// <param name="city">The city</param>
        /// <exception cref="ArgumentException">Thrown when city is invalid</exception>
        public void SetCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be null or empty.", nameof(city));
            }

            if (city.Length > 100)
            {
                throw new ArgumentException("City cannot exceed 100 characters.", nameof(city));
            }

            _city = city.Trim();
        }

        /// <summary>
        /// Sets the state or province with validation
        /// </summary>
        /// <param name="state">The state or province</param>
        /// <exception cref="ArgumentException">Thrown when state is invalid</exception>
        public void SetState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
            {
                throw new ArgumentException("State cannot be null or empty.", nameof(state));
            }

            if (state.Length > 100)
            {
                throw new ArgumentException("State cannot exceed 100 characters.", nameof(state));
            }

            _state = state.Trim();
        }

        /// <summary>
        /// Sets the postal code with validation
        /// </summary>
        /// <param name="postalCode">The postal or ZIP code</param>
        /// <exception cref="ArgumentException">Thrown when postal code is invalid</exception>
        public void SetPostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ArgumentException("Postal code cannot be null or empty.", nameof(postalCode));
            }

            if (postalCode.Length > 20)
            {
                throw new ArgumentException("Postal code cannot exceed 20 characters.", nameof(postalCode));
            }

            _postalCode = postalCode.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Sets the country with validation
        /// </summary>
        /// <param name="country">The country</param>
        /// <exception cref="ArgumentException">Thrown when country is invalid</exception>
        public void SetCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));
            }

            if (country.Length > 100)
            {
                throw new ArgumentException("Country cannot exceed 100 characters.", nameof(country));
            }

            _country = country.Trim();
        }

        /// <summary>
        /// Returns a formatted address string
        /// </summary>
        /// <returns>Formatted address</returns>
        public override string ToString()
        {
            return $"{_street}, {_city}, {_state} {_postalCode}, {_country}";
        }

        /// <summary>
        /// Validates that all required address fields are set
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_street) &&
                   !string.IsNullOrWhiteSpace(_city) &&
                   !string.IsNullOrWhiteSpace(_state) &&
                   !string.IsNullOrWhiteSpace(_postalCode) &&
                   !string.IsNullOrWhiteSpace(_country);
        }
    }
}
