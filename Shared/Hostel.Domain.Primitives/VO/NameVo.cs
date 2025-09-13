using Hostel.Shared.Kernel;

namespace Hostel.Domain.Primitives
{
    /// <summary>
    /// Название (Value Object)
    /// </summary>
    public class NameVo : ValueObject
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
        /// Название
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Название
        /// </summary>
        /// <param name="value">Хначение</param>
        public NameVo(string value)
        {
            Value = SetCharField(PrimitivesFieldCodes.PrimitiveFieldName, value, MinLength, MaxLength);
        }

        /// <summary>
        /// Преобразование в строковый формат
        /// </summary>
        /// <returns>Строка</returns>
        public override string ToString() => Value;

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
