namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Корень аггрегата
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : notnull
    {
        /// <summary>
        /// Список событий
        /// </summary>
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        /// <summary>
        /// Конструктор корня агрегата
        /// </summary>
        /// <param name="id">Идентификатор</param>
        protected AggregateRoot(TKey id) : base(id)
        {
        }

        /// <summary>
        /// Список событий
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Добавить событие
        /// </summary>
        /// <param name="domainEvent">Событие</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Очистка событий
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
