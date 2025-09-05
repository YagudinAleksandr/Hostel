namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Базовая сущность
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>> where TKey : notnull
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public TKey Id { get; private set; }

        /// <summary>
        /// Конструктор сущности
        /// </summary>
        /// <param name="id">Идентификатор</param>
        protected Entity(TKey id)
        {
            Id = id ?? throw new DomainRequiredFieldException(DomainFieldCodes.DomainFieldId);
        }

        /// <summary>
        /// Сравнение объекта
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns>true - равны, false - не равны</returns>
        public override bool Equals(object? obj)
        {
            return obj is Entity<TKey> other && Id.Equals(other.Id);
        }

        /// <summary>
        /// Сравнение объекта
        /// </summary>
        /// <param name="other">Объект</param>
        /// <returns>true - равны, false - не равны</returns>
        public bool Equals(Entity<TKey>? other)
        {
            return other is not null && Id.Equals(other.Id);
        }

        /// <summary>
        /// Получение хэш-суммы объекта
        /// </summary>
        /// <returns>Хэш-сумма</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Оператор РАВНЫ
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>true - равны, false - не равны</returns>
        public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Оператор НЕ
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>true - не равны, false - равны</returns>
        public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
        {
            return !Equals(left, right);
        }
    }
}
