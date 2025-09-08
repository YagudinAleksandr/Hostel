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
            string name = "Test name";
            var nameVo = new NameVo(name);

            Assert.NotNull(nameVo);
            Assert.Equal(nameVo.Value, name);
        }

        [Fact(DisplayName = "Возвращает исключение о незаполненном поле")]
        public async Task Should_Return_Domain_Required_Field_Exception()
        {
            string name = "";
            var nameVo = () => new NameVo(name);

            nameVo.Should().Throw<DomainRequiredFieldException>();
        }

        [Fact(DisplayName = "Возвращает исключение о минимальной длине поля")]
        public async Task Should_Return_Domain_Min_Length_Field_Exception()
        {
            string name = "t";
            var nameVo = () => new NameVo(name);

            nameVo.Should().Throw<DomainMinLengthFieldException>();
        }

        [Fact(DisplayName = "Возвращает исключение о максимальной длине поля")]
        public async Task Should_Return_Domain_Max_Length_Field_Exception()
        {
            string name = new string('x',70);
            var nameVo = () => new NameVo(name);

            nameVo.Should().Throw<DomainMaxLengthFieldException>();
        }
    }
}
