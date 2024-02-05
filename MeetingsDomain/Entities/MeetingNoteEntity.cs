using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsDomain.Entities
{
    public class MeetingNoteEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int MeetingId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        [ForeignKey(nameof(MeetingId))]
        public virtual MeetingEntity Meeting { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public virtual UserEntity Author { get; set; }

        public MeetingNoteEntity()
        {
            CreationDate = DateTime.Now;
        }
    }
}
