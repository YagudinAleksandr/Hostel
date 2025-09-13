namespace Hostel.SU.Application
{
    /// <summary>
    /// Сервис для обработки сброса пароля пользователем
    /// </summary>
    public interface IResetUserPasswordService
    {
        /// <summary>
        /// Формирование токена сброса пароля
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>true - сформирован, false - ошибка</returns>
        Task CreateResetPasswordAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="token">Токен сброса</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>true - удачный сброс пароля</returns>
        Task ChangePasswordAsync(Guid token, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Пометить токены как истекшие
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SetExpiredTokenAsync(CancellationToken cancellationToken = default);
    }
}
