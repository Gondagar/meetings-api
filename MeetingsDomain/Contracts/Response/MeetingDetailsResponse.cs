using MeetingsDomain.Entities;
using System;
using System.Collections.Generic;

namespace MeetingsDomain.Contracts.Response
{
    public class MeetingDetailsResponse
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public decimal Duration { get; set; }

        public bool IsOwner { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime CreationDate { get; set; }

        public List<MeetingNoteResponse> Notes { get; set; }

        public List<MeetingParticipantResponse> Participants { get; set; }
    }
}
