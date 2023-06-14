namespace Hostel.Domain.DTO.UsersDTOs
{
    public class UserLoginResponseDTO
    {
        /// <summary>
        /// Выполнена ли авторизация
        /// </summary>
        public bool IsAuthSuccessful { get; set; }
        /// <summary>
        /// Ошибки при авторизации
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Сессия
        /// </summary>
        public string SessionID { get; set; }
    }
}
