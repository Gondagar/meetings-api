using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsDomain.Entities
{
    public class MeetingParticipantEntity
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MeetingId { get; set; }

        /// <summary>
        /// The user being added
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// The meeting being added to
        /// </summary>
        [ForeignKey(nameof(MeetingId))]
        public virtual MeetingEntity Meeting { get; set; }

        /// <summary>
        /// Identifier if the meeting has been created by this participant
        /// </summary>
        [Required]
        public bool IsOwner { get; set; }

        public MeetingParticipantEntity()
        {
            IsOwner = false;
        }
    }
}
