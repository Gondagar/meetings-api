using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingsDomain.Contracts.Response
{
    public class MeetingEmailParticipantResponse
    {
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserEmail { get; set; }

        public bool IsOwner { get; set; }
    }
}
