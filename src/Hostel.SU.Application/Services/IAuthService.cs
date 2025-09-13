using Hostel.Users.Contracts.Response;

namespace Hostel.SU.Application
{ 
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Выполнение авторизации
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="password">Пароль</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ авторизации</returns>
        Task<UserLoginResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление токена
        /// </summary>
        /// <param name="refreshToken">Токен обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ авторизации</returns>
        Task<UserLoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Отзыв токена обновления
        /// </summary>
        /// <param name="refreshToken">Токен обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверка на отозванный токен
        /// </summary>
        /// <param name="refreshToken">Токен обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>true - отозван, false - не отозван</returns>
        Task<bool> IsTokenRevokedAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
