namespace Hostel.Domain.DTO.UsersDTOs
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string NormilizedEmail { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public string NormalizedUserName { get; set; }
        public string Fullname { get; set; }
        public string ProfileImg { get; set; }
    }
}
