using FluentAssertions;
using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SharedPrimitives.Tests
{
    /// <summary>
    /// Тесты для Value Object <see cref="AddressVo"/>
    /// </summary>
    public class AddressVoTests
    {
        [Theory(DisplayName = "Создание адреса")]
        [InlineData("124586", "Russia", "Moscow region", "Moscow", "Test", "584")]
        [InlineData("124586", "Russia", null, "Moscow", "Test", "584")]
        public async Task Should_Create_Address(string postalCode, string country, string? region, string city, string street, string house)
        {
            // Act
            var address = new AddressVo(postalCode, country, region, city, street, house);

            // Assert
            Assert.NotNull(address);
            Assert.Equal(country, address.Country);
            Assert.Equal(postalCode, address.PostalCode);
            Assert.Equal(city, address.City);
            Assert.Equal(street, address.Street);
            Assert.Equal(house, address.House);
        }

        [Theory(DisplayName = "Создание адреса должно вернуть исключение обязательных полей")]
        [InlineData("", "Russia", "Moscow region", "Moscow", "Test", "584")]
        [InlineData("124586", "Russia", null, "", "Test", "584")]
        public async Task Should_Return_Required_Field_Exception(string postalCode, string country, string? region, string city, string street, string house)
        {
            // Act
            var address = () => new AddressVo(postalCode, country, region, city, street, house);

            // Assert
            address.Should().Throw<DomainRequiredFieldException>();
        }

        [Fact(DisplayName = "Создание адреса должно вернуть исключение о минимальной длине строки")]
        public async Task Should_Return_Min_Length_Field_Exception()
        {
            // Act
            var address = () => new AddressVo("544465", "Russia", "Test", "M", "Street", "House");

            // Assert
            address.Should().Throw<DomainMinLengthFieldException>();
        }

        [Fact(DisplayName = "Создание адреса должно вернуть исключение о максимальной длине строки")]
        public async Task Should_Return_Max_Length_Field_Exception()
        {
            // Act
            var address = () => new AddressVo("544465", "Russia", "Test", new string('A', 65), "Street", "House");

            // Assert
            address.Should().Throw<DomainMaxLengthFieldException>();
        }

        [Fact(DisplayName = "Должно вернуть исключение о невалидном заполнении поля")]
        public async Task Should_Return_Validation_Field_Exception()
        {
            // Act
            var address = () => new AddressVo("544d65", "Russia", "Test", new string('A', 65), "Street", "House");

            // Assert
            address.Should().Throw<DomainValidationFieldException>();
        }

        [Theory(DisplayName = "Строковое представление адреса")]
        [InlineData("544465, Country, Region, City, Street, House", "Region")]
        [InlineData("544465, Country, City, Street, House", null)]
        public async Task Should_Return_String_Address(string result, string? region)
        {
            // Arrange
            var address = new AddressVo("544465", "Country", region, "City", "Street", "House");

            // Act
            string addressString = address.ToString();

            // Assert
            Assert.Equal(result, addressString);
        }
    }
}
