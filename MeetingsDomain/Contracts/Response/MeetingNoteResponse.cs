using System;

namespace MeetingsDomain.Contracts.Response
{
    public class MeetingNoteResponse
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public UserResponse Author { get; set; }
    }
}
