namespace Hostel.SU.Domain
{
    /// <summary>
    /// Сервис генерации токенов
    /// </summary>
    public interface ITokenGeneratorService
    {
        /// <summary>
        /// Время жизни токена обновления
        /// </summary>
        int RefreshTokenLifetimeMinutes { get; }

        /// <summary>
        /// Время жизни токена доступа
        /// </summary>
        int AccessTokenLifetimeMinutes { get; }

        /// <summary>
        /// Генерация токена дочтупа
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="type">Тип пользователя</param>
        /// <returns>Токен доступа</returns>
        string GenerateAccessToken(Guid userId, UserType type);

        /// <summary>
        /// Генерация токена обновления
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Токен обновления</returns>
        string GenerateRefreshToken(Guid userId);
    }
}
