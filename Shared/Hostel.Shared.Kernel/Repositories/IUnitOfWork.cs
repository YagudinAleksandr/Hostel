namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Начать транзакцию (если поддерживается)
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        Task BeginTransactionAsync(CancellationToken ct = default);

        /// <summary>
        /// Сохранить изменения (в транзакции или без)
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        Task CommitAsync(CancellationToken ct = default);

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        Task RollbackAsync(CancellationToken ct = default);
    }
}
