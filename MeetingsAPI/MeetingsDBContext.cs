using MeetingsDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingsAPI
{
    public class MeetingsDBContext : DbContext
    {
        // таблички бази даних
        public DbSet<MeetingEntity> Meetings { get; set; }
        
        public DbSet<MeetingNoteEntity> MeetingNotes { get; set; }

        public DbSet<MeetingParticipantEntity> MeetingParticipants { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public MeetingsDBContext(DbContextOptions<MeetingsDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
        }
    }
}
