using MeetingsDomain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetingsDomain.Repositories
{
    public interface IMeetingRepository
    {
        Task<List<MeetingEntity>> GetAll(int userId, DateTime startTime, DateTime endTime);

        Task<List<MeetingEntity>> GetAll(DateTime startTime);

        Task<MeetingEntity> GetSingle(int id);

        Task<int> Add(MeetingEntity meeting);

        Task<int> Delete(MeetingEntity meeting);

        Task<List<MeetingNoteEntity>> GetAllNotes(int meetingId, int offset, int limit);

        Task<MeetingNoteEntity> GetSingleNote(int meetingId, int id);

        Task<int> AddNote(MeetingNoteEntity note);

        Task<int> DeleteNote(MeetingNoteEntity note);

        Task<MeetingParticipantEntity> GetSingleParticipant(int meetingId, int id);

        Task<List<MeetingParticipantEntity>> GetParticipants(int meetingId);
        

        Task<int> AddParticipants(List<MeetingParticipantEntity> participants);

        Task<int> DeleteParticipant(MeetingParticipantEntity participant);
    }
}
