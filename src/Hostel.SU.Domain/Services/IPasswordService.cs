namespace Hostel.SU.Domain
{
    /// <summary>
    /// Сервис для хэширования паролей
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Хэширование пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Хэш пароля</returns>
        string GetHashPassword(string password);

        /// <summary>
        /// Валидация хэша пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="hash">Хэш пароля</param>
        /// <returns>true - валиден, false - не валиден</returns>
        bool ValidateHash(string password, string hash);
    }
}
