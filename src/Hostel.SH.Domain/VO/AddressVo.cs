using Hostel.Shared.Kernel;

namespace Hostel.SH.Domain
{
    /// <summary>
    /// Адрес VO
    /// </summary>
    public class AddressVo : ValueObject
    {
        /// <summary>
        /// Минимальная длина строки
        /// </summary>
        private const int MinLength = 2;

        /// <summary>
        /// Максимальная длина строки
        /// </summary>
        private const int MaxLength = 30;

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostalCode { get; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; }

        /// <summary>
        /// Регион
        /// </summary>
        public string? Region { get; }

        /// <summary>
        /// Город
        /// </summary>
        public string City { get; }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; }

        /// <summary>
        /// Строение
        /// </summary>
        public string House { get; }

        /// <summary>
        /// Адрес
        /// </summary>
        /// <param name="postalCode">Почтовый индекс</param>
        /// <param name="country">Страна</param>
        /// <param name="city">Город</param>
        /// <param name="street">Улица</param>
        /// <param name="house">Строение</param>
        /// <param name="region">Регион</param>
        public AddressVo(string postalCode, string country, string city, string street, string house, string? region)
        {
            if (!postalCode.All(char.IsDigit))
                throw new DomainException("Not number", postalCode);

            PostalCode = SetField(postalCode, MinLength, MaxLength);
            Country = SetField(country, MinLength, MaxLength);
            City = SetField(city, MinLength, MaxLength);
            Street = SetField(street, MinLength, MaxLength);
            House = SetField(house, MinLength, MaxLength);
            Region = region;
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PostalCode;
            yield return Country;
            yield return City;
            yield return Street;
            yield return House;
            yield return Region ?? string.Empty;
        }
    }
}
