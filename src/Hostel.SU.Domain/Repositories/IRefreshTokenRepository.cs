namespace Hostel.SU.Domain
{
    /// <summary>
    /// Репозиторий для работы с токенами обновления
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Добавить токен
        /// </summary>
        /// <param name="token">Токен обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Токен обновления</returns>
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken = default);
    }
}
