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
        /// Установка значения поля
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="minLength">Минимальная длина</param>
        /// <param name="maxLength">Максимальная длина</param>
        /// <returns>Значение</returns>
        /// <exception cref="DomainException">Ошибка валидации поля</exception>
        public string SetField(string value, int minLength, int maxLength)
        {
            if (!string.IsNullOrEmpty(value))
                throw new DomainException("RequiredFiled", "Name");

            if (value.Length < minLength)
                throw new DomainException("MinLength", "Name");

            if (value.Length > maxLength)
                throw new DomainException("MaxLength", "Name");

            return value;
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
