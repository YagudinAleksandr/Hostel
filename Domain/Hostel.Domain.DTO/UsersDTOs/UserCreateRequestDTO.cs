using System;
using System.ComponentModel.DataAnnotations;

namespace Hostel.Domain.DTO.UsersDTOs
{
    public class UserCreateRequestDTO
    {
        [Required(ErrorMessage ="Поле Фамилия Имя Отчество должно быть запонено")]
        public string Fullname { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImg { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        [Required(ErrorMessage ="Поле E-mail должно быть заполнено")]
        [EmailAddress(ErrorMessage ="E-mail должен быть приведен к адресу электронной почты")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Поле Пароль должно быть заполнено")]
        [MinLength(8,ErrorMessage ="Поле пароль должно содержать минимально 8 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage ="Пароли не совпадают")]
        public string ConfirmedPassword { get; set; }
        [Required(ErrorMessage = "Не указано полное имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Поле В лице должно быть заполнено")]
        public string Post { get; set; }
        [Required(ErrorMessage = "Поле Контактный телефон должно быть заполнено")]
        public string Phone { get; set; }
    }
}
