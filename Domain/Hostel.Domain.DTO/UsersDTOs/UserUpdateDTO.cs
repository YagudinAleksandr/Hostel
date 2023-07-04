using System.ComponentModel.DataAnnotations;

namespace Hostel.Domain.DTO.UsersDTOs
{
    public class UserUpdateDTO
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImg { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле В лице должно быть заполнено")]
        public string Post { get; set; }
        [Required(ErrorMessage = "Поле Контактный телефон должно быть заполнено")]
        public string Phone { get; set; }
    }
}
