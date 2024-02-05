using System.ComponentModel.DataAnnotations;

namespace MeetingsDomain.Contracts.Request
{
    public class AddMeetingNoteRequest
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
    }
}
