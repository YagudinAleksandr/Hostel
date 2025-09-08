using FluentAssertions;
using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SharedPrimitives.Tests
{
    /// <summary>
    /// Тесты для Value Object <see cref="FullNameVo"/>
    /// </summary>
    public class FullNameVoTests
    {
        [Theory(DisplayName = "Создание ФИО")]
        [InlineData("Тест", "Test", "Test")]
        [InlineData("Тест", "Тест", null)]
        public async Task Should_Create_Full_Name(string lastname, string firstname, string? patronymic)
        {
            // Act
            var fullname = new FullNameVo(firstname, lastname, patronymic);

            // Assert
            Assert.NotNull(fullname);
            Assert.Equal(firstname, fullname.FirstName);
            Assert.Equal(lastname, fullname.LastName);
        }

        [Theory(DisplayName = "Должен вернуть исключение о заполнении обязательных полей")]
        [InlineData("Тест", "", null)]
        [InlineData("", "Тест", null)]
        public async Task Should_Return_Required_Field_Exception(string lastname, string firstname, string? patronymic)
        {
            // Act
            var fullname = () => new FullNameVo(firstname, lastname, patronymic);

            // Arrange
            fullname.Should().Throw<DomainRequiredFieldException>();
        }

        [Fact(DisplayName = "Должен вернуть исключение о максимальной длине полей")]
        public async Task Should_Return_Max_Length_Field_Exception()
        {
            // Arrange
            string firstname = new string('t', 60);

            // Act
            var fullname = () => new FullNameVo(firstname, "Test", "Test");

            // Arrange
            fullname.Should().Throw<DomainMaxLengthFieldException>();
        }
    }
}
