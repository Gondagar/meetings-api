using MeetingsDomain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MeetingsDomain.Services
{
    public interface IMeetingEmailsService
    {
        void Send(MeetingEntity meeting, MeetingParticipantEntity meetingParticipant);
    }
}
