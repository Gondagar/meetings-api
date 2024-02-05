using System;
using System.Collections.Generic;

namespace MeetingsDomain.Contracts.Response
{
    public class MeetingEmailDetailsResponse
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public List<MeetingEmailParticipantResponse> Participants { get; set; }

        public MeetingEmailParticipantResponse Owner { get; set; }
    }
}
