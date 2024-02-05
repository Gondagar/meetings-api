using MeetingsDomain.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsDomain.Entities
{
    public class MeetingEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [Required]
        [Range(0, 12)]
        public int Duration { get; set; }

        [Required]
        [StartTime]
        public DateTime StartTime { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual List<MeetingNoteEntity> Notes { get; set; }

        public virtual List<MeetingParticipantEntity> Participants { get; set; }

        public MeetingEntity()
        {
            CreationDate = DateTime.Now;
        }
    }
}
