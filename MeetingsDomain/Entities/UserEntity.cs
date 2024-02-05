using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingsDomain.Entities
{
    /// <summary>
    /// User entity
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// Unique user id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// User password hash
        /// </summary>
        [Required]
        [MaxLength(2048)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// User first name
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        /// <summary>
        /// User lastname
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        /// <summary>
        /// Path to the user avatar file
        /// </summary>
        [MaxLength(2048)]
        public string AvatarUrl { get; set; }
        
        public virtual List<MeetingParticipantEntity> Participants { get; set; }

        public virtual List<MeetingNoteEntity> Notes { get; set; }
    }
}
