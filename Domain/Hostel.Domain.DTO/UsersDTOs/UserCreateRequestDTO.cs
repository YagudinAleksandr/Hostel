using System;

namespace Hostel.Domain.DTO.UsersDTOs
{
    public class UserCreateRequestDTO
    {
        public string Fullname { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImg { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
