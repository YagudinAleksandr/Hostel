namespace Hostel.Domain.Security
{
    /// <summary>
    /// Идентификация и создание пароля
    /// </summary>
    public static class PasswordIdentification
    {
        /// <summary>
        /// Получение хэша пароля
        /// </summary>
        /// <param name="password">Пароль в открытом виде</param>
        /// <returns>Хэш пароля</returns>
        public static string GetHashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Проверка валидности пароля
        /// </summary>
        /// <param name="requestPassword">Запрашиваемое значение пароля</param>
        /// <param name="modelPassword">Хэш модели пароля</param>
        /// <returns>True - совпадает хэш, False - не совпадает</returns>
        public static bool GetValidPassword(string requestPassword, string modelPassword)
        {
            return BCrypt.Net.BCrypt.Verify(requestPassword, modelPassword);
        }
    }
}
