using System.ComponentModel.DataAnnotations;
namespace PSWeb_Server.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "UserID is Required")]
        public int ID { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        
        public decimal Fund { get; set;}

    }
}