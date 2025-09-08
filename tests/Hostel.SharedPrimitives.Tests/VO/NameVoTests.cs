using FluentAssertions;
using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SharedPrimitives.Tests
{
    /// <summary>
    /// Тесты на VO <see cref="NameVo"/>
    /// </summary>
    public class NameVoTests
    {
        [Fact(DisplayName = "Формирование названия")]
        public async Task Should_Create_Name()
        {
            // Arrange
            string name = "Test name";

            // Act
            var nameVo = new NameVo(name);

            // Assert
            Assert.NotNull(nameVo);
            Assert.Equal(nameVo.Value, name);
        }

        [Fact(DisplayName = "Возвращает исключение о незаполненном поле")]
        public async Task Should_Return_Domain_Required_Field_Exception()
        {
            // Arrange
            string name = "";

            // Act
            var nameVo = () => new NameVo(name);

            // Assert
            nameVo.Should().Throw<DomainRequiredFieldException>();
        }

        [Fact(DisplayName = "Возвращает исключение о минимальной длине поля")]
        public async Task Should_Return_Domain_Min_Length_Field_Exception()
        {
            // Arrange
            string name = "t";

            // Act
            var nameVo = () => new NameVo(name);

            // Assert
            nameVo.Should().Throw<DomainMinLengthFieldException>();
        }

        [Fact(DisplayName = "Возвращает исключение о максимальной длине поля")]
        public async Task Should_Return_Domain_Max_Length_Field_Exception()
        {
            // Arrange
            string name = new string('x',70);

            // Act
            var nameVo = () => new NameVo(name);

            // Assert
            nameVo.Should().Throw<DomainMaxLengthFieldException>();
        }
    }
}
