using Hostel.Domain.Primitives;

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
            string name = "Test name";
            var nameVo = new NameVo(name);

            Assert.NotNull(nameVo);
            Assert.Equal(nameVo.Value, name);
        }

    }
}
