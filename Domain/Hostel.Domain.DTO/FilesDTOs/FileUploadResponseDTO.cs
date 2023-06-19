namespace Hostel.Domain.DTO.FilesDTOs
{
    public class FileUploadResponseDTO
    {
        public bool IsSuccesful { get; set; }
        public string Errors { get; set; }
        public string FileName { get; set; }
    }
}
