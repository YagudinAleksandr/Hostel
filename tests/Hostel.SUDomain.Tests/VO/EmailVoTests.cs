using FluentAssertions;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;

namespace Hostel.SUDomain.Tests.VO
{
    /// <summary>
    /// Тесты для Value Object <see cref="EmailVo"/>
    /// </summary>
    public class EmailVoTests
    {
        [Fact(DisplayName = "Создание адреса электронной почты")]
        public async Task Should_Create_Email()
        {
            // Arrange
            string email = "test@test.local";

            // Act
            var result = new EmailVo(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Value);
        }

        [Fact(DisplayName = "Исключение о минимальной длине поля")]
        public async Task Should_Return_Min_Length_Field_Exception()
        {
            // Arrange
            string email = "t@tl";

            // Act
            var result = () => new EmailVo(email);

            // Assert
            result.Should().Throw<DomainMinLengthFieldException>();
        }

        [Fact(DisplayName = "Исключение о максимальной длине поля")]
        public async Task Should_Return_Max_Length_Field_Exception()
        {
            // Arrange
            string email = new string('A', 62);

            // Act
            var result = () => new EmailVo(email);

            // Assert
            result.Should().Throw<DomainMaxLengthFieldException>();
        }

        [Theory(DisplayName = "Исключение о невалидном адресе электроннйо почты")]
        [InlineData("a@b.c")]
        [InlineData("a@b")]
        [InlineData("a@.com")]
        [InlineData("plainaddress")]
        [InlineData("user@domain")]
        [InlineData("user@domain.c")]
        public async Task Should_Return_Not_Valid_Exception(string input)
        {
            if (input != null && input.Length >= 5 && input.Length <= 60)
            {
                Assert.Throws<DomainValidationFieldException>(() => new EmailVo(input));
            }
        }
    }
}
