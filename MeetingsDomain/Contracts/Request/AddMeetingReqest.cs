using MeetingsDomain.Attribute;
using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingsDomain.Contracts.Request
{
    public class AddMeetingReqest
    {
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [Required]
        [StartTime]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(0, 12)]
        public decimal Duration { get; set; }
    }
}
