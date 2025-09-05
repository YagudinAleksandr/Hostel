namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Базовый Value Object
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Компоненты для сравнения
        /// </summary>
        /// <returns>Список параметров для сравнения</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Присвоение значения
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="minLength">Минимальная длина</param>
        /// <param name="maxLength">Максимальная длина</param>
        /// <param name="fieldNameCode">Код поля</param>
        /// <returns>Значение</returns>
        /// <exception cref="DomainRequiredFieldException"></exception>
        /// <exception cref="DomainMinLengthFieldException"></exception>
        /// <exception cref="DomainMaxLengthFieldException"></exception>
        protected static string SetCharField(string fieldNameCode, string value, int minLength, int maxLength)
        {
            if (!string.IsNullOrEmpty(value))
                throw new DomainRequiredFieldException(fieldNameCode);

            if (value.Length < minLength)
                throw new DomainMinLengthFieldException(fieldNameCode, minLength);

            if (value.Length > maxLength)
                throw new DomainMaxLengthFieldException(fieldNameCode, maxLength);

            return value;
        }

        /// <summary>
        /// присвоение числового значения
        /// </summary>
        /// <param name="fieldNameCode">Код поля</param>
        /// <param name="value">Значение</param>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        /// <returns>Значение</returns>
        /// <exception cref="DomainRangeFieldException"></exception>
        /// <exception cref="DomainMaxLengthFieldException"></exception>
        /// <exception cref="DomainMinLengthFieldException"></exception>
        protected static double SetNumericField(string fieldNameCode,
            double value,
            double? minValue = null,
            double? maxValue = null)
        {
            if (minValue != null
                && maxValue != null
                && (value < minValue || value > maxValue))
                throw new DomainRangeFieldException(fieldNameCode, minValue.Value, maxValue.Value);

            if (minValue != null && maxValue == null && value < minValue)
                throw new DomainMinLengthFieldException(fieldNameCode, minValue.Value);

            if (maxValue != null && minValue == null && value > maxValue)
                throw new DomainMaxLengthFieldException(fieldNameCode, maxValue.Value);

            return value;
        }

        /// <summary>
        /// Проверка равенства
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns>true - равны, false - не равны</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not ValueObject other)
                return false;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// Сравнение объекта
        /// </summary>
        /// <param name="other">Объект</param>
        /// <returns>true - равны, false - не равны</returns>
        public bool Equals(ValueObject? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            // Сравниваем по всем значимым полям
            return GetEqualityComponents()
                .SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// Получение хэш суммы объекта
        /// </summary>
        /// <returns>Хэш-сумма</returns>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// Оператор РАВНЫ
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>true - равны, false - не равны</returns>
        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Оператор НЕ
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>true - не равны, false - равны</returns>
        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !Equals(left, right);
        }
    }
}
