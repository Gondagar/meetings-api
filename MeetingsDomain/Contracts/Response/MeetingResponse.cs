using MeetingsDomain.Entities;
using System;
using System.Collections.Generic;

namespace MeetingsDomain.Contracts.Response
{
    public class MeetingResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Duration { get; set; }

        public DateTime StartTime { get; set; }

        public List<MeetingParticipantResponse> Participants { get; set; }
    }
}
