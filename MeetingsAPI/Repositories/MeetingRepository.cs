using MeetingsDomain.Entities;
using MeetingsDomain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingsAPI.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private MeetingsDBContext _context;

        public MeetingRepository(MeetingsDBContext context)
        {
            _context = context;
        }

        public Task<int> Add(MeetingEntity model)
        {
            _context.Meetings.Add(model);

            return _context.SaveChangesAsync();
        }

        public Task<int> Delete(MeetingEntity meeting)
        {
            _context.Remove(meeting);

            return _context.SaveChangesAsync();
        }

        public Task<List<MeetingEntity>> GetAll(DateTime startTime)
        {
            return _context
                .Meetings
                .Where(meeting => meeting.StartTime >= startTime)
                .Include(meeting => meeting.Participants)
                .ThenInclude(participant => participant.User)
                .ToListAsync();
        }

        public Task<List<MeetingEntity>> GetAll(int userId, DateTime startTime, DateTime endTime)
        {
            return _context
                .Meetings
                .Where(meeting => 
                    meeting.StartTime >= startTime && 
                    meeting.StartTime <= endTime &&
                    meeting.Participants.Where(participant => participant.UserId == userId).Any()
                  )
                .ToListAsync();
        }

        public Task<MeetingEntity> GetSingle(int id)
        {
            return _context
                .Meetings
                .Include(meeting => meeting.Notes)
                    .ThenInclude(note => note.Author)
                .Include(meeting => meeting.Participants)
                    .ThenInclude(participant => participant.User)
                .Where(meeting => meeting.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<List<MeetingNoteEntity>> GetAllNotes(int meetingId, int offset = 0, int limit = 10)
        {
            return _context
                        .MeetingNotes
                        .Where(meetingNote => meetingNote.Meeting.Id == meetingId)
                        .OrderByDescending(mm => mm.CreationDate)
                        .Skip(offset * limit)
                        .Take(limit)
                        .ToListAsync();
        }

        public Task<MeetingNoteEntity> GetSingleNote(int meetingId, int id)
        {
            return _context
                .MeetingNotes
                .FirstOrDefaultAsync(note => note.Id == id && note.Meeting.Id == meetingId);
        }

        public Task<int> AddNote(MeetingNoteEntity note)
        {
            _context.MeetingNotes.Add(note);

            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteNote(MeetingNoteEntity note)
        {
            _context.Remove(note);

            return _context.SaveChangesAsync();
        }

        public Task<int> AddParticipants(List<MeetingParticipantEntity> participants)
        {
            foreach (var participant in participants)
            {
                _context.MeetingParticipants.Add(participant);
            }

            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteParticipant(MeetingParticipantEntity participant)
        {
            _context.Remove(participant);

            return _context.SaveChangesAsync();
        }

        public Task<MeetingParticipantEntity> GetSingleParticipant(int meetingId, int id)
        {
            return _context
                .MeetingParticipants
                .Include(participant => participant.User)
                .FirstOrDefaultAsync(participant => participant.Id == id && participant.MeetingId == meetingId);
        }

        public Task<List<MeetingParticipantEntity>> GetParticipants(int meetingId)
        {
            return _context
                .MeetingParticipants
                .Where(participant => participant.Meeting.Id == meetingId)
                .Include(participant => participant.User)
                .ToListAsync();
        }
    }
}