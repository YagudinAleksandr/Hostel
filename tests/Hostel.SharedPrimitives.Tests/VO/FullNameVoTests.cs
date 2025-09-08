using Hostel.Domain.Primitives;

namespace Hostel.SharedPrimitives.Tests
{
    /// <summary>
    /// Тесты для Value Object <see cref="FullNameVo"/>
    /// </summary>
    public class FullNameVoTests
    {
        [Theory(DisplayName = "")]
        [InlineData("Тест", "Test", "Test")]
        [InlineData("Тест", "Тест", null)]
        public async Task Should_Create_Full_Name(string lastname, string firstname, string? patronymic)
        {
            var fullname = new FullNameVo(firstname, lastname, patronymic);

            Assert.NotNull(fullname);
            Assert.Equal(firstname, fullname.FirstName);
            Assert.Equal(lastname, fullname.LastName);
        }
    }
}
