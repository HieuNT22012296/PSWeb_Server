namespace PSWeb_Server.DTOs.UserDTOs
{
    public class UpdateUserResponseDTO
    {
        public int userID { get; set; }
        public string Message { get; set; }
        public bool IsUpdated { get; set; }
    }
}