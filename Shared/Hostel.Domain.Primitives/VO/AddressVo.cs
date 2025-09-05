using Hostel.Shared.Kernel;

namespace Hostel.Domain.Primitives
{
    /// <summary>
    /// Адрес (Value Object)
    /// </summary>
    public class AddressVo : ValueObject
    {
        /// <summary>
        /// Минимальная длина
        /// </summary>
        private const int MinLength = 2;

        /// <summary>
        /// Максимальная длина
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
        /// <param name="region">Регион</param>
        /// <param name="city">Город</param>
        /// <param name="street">Улица</param>
        /// <param name="house">Строение</param>
        /// <exception cref="DomainValidationFieldException"></exception>
        public AddressVo(string postalCode, string country, string? region, string city, string street, string house)
        {
            PostalCode = SetCharField(PrimitivesFieldCodes.PrimitiveFieldPostalCode, postalCode, MinLength, MaxLength);

            if (!postalCode.All(char.IsDigit))
                throw new DomainValidationFieldException(PrimitivesFieldCodes.PrimitiveFieldPostalCode, "0-9");

            Country = SetCharField(PrimitivesFieldCodes.PrimitiveFieldCountry, country, MinLength, MaxLength);

            if(!string.IsNullOrEmpty(region))
                Region = SetCharField(PrimitivesFieldCodes.PrimitiveFieldRegion, region, MinLength, MaxLength);

            City = SetCharField(PrimitivesFieldCodes.PrimitiveFieldCity, city, MinLength, MaxLength);
            Street = SetCharField(PrimitivesFieldCodes.PrimitiveFieldStreet, street, MinLength, MaxLength);
            House = SetCharField(PrimitivesFieldCodes.PrimitiveFieldHouse, house, MinLength, MaxLength);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PostalCode;
            yield return Country;
            yield return Region ?? string.Empty;
            yield return City;
            yield return Street;
            yield return House;
        }

        /// <summary>
        /// преобразование в строку
        /// </summary>
        /// <returns>Строка адреса</returns>
        public override string ToString()
        {
            string address = $"{PostalCode}, {Country}, ";
            address += !string.IsNullOrEmpty(Region) ? $" {Region}, " : string.Empty;
            address += $"{City}, {Street}, {House}";

            return address;
        }
    }
}
