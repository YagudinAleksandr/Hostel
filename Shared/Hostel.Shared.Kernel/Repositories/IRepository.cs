namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Базовый репозиторий
    /// </summary>
    /// <typeparam name="TAggregate">Агрегат</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    public interface IRepository<TAggregate, TKey> where TAggregate : AggregateRoot<TKey> where TKey : notnull
    {
        /// <summary>
        /// Добавление агрегата
        /// </summary>
        /// <param name="aggregate">Агрегат</param>
        void Add(TAggregate aggregate);

        /// <summary>
        /// Обновление агрегата
        /// </summary>
        /// <param name="aggregate">Агрегат</param>
        void Update(TAggregate aggregate);

        /// <summary>
        /// Удаление агрегата
        /// </summary>
        /// <param name="aggregate">Агрегат</param>
        void Delete(TAggregate aggregate);

        /// <summary>
        /// Получение агрегата по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken">Токент отмены</param>
        /// <returns>Агрегат</returns>
        Task<TAggregate?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение всех агрегатов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Агрегаты</returns>
        Task<IQueryable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
