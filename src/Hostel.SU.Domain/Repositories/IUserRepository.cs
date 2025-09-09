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
        /// <returns>true - существует, false - отсутствует</returns>
        bool IsEmailExists(string email, Guid? skipUserId = null);
    }
}
