using System.ComponentModel.DataAnnotations;

namespace MeetingsDomain.Contracts.Request
{
    public class LoginUserRequest
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
