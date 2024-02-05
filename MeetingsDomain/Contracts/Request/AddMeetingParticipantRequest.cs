using System.ComponentModel.DataAnnotations;

namespace MeetingsDomain.Contracts.Request
{
    public class AddMeetingParticipantRequest
    {
        [Required]
        public int UserId { get; set; }
    }
}
