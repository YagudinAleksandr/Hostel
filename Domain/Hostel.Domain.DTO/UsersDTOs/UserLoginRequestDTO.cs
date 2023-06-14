using System.ComponentModel.DataAnnotations;

namespace Hostel.Domain.DTO.UsersDTOs
{
    /// <summary>
    /// Класс пользователя на запрос авторизации
    /// </summary>
    public class UserLoginRequestDTO
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessage ="Поле E-mail должно быть заполнено")]
        [EmailAddress(ErrorMessage = "Неверный стандарт записи почты")]
        public string Username { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Поле Пароль должно быть заполнено")]
        public string Password { get; set; }
        /// <summary>
        /// Является ли запрос с сервера
        /// </summary>
        public bool IsServer { get; set; }
    }
}
