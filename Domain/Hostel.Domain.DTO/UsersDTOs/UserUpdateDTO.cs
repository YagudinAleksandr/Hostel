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
    }
}
