namespace PSWeb_Server.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public decimal Fund { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn{ get; set;}
    }
}