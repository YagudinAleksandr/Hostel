using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Репозиторий пользователя
    /// </summary>
    public interface IUserRepository : IRepository<User, Guid>
    {
        /// <summary>
        /// Проверка на существание адреса электронной почты
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="skipUserId">Идентификатор пользователя для пропуска в поиске</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>true - существует, false - отсутствует</returns>
        Task<bool> IsEmailExistsAsync(string email, Guid? skipUserId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение по адресу электронной почты
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Пользователь</returns>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
