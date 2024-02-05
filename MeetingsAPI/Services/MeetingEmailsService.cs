using AutoMapper;
using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Entities;
using MeetingsDomain.Services;
using System;

namespace MeetingsAPI.Services
{
    public class MeetingEmailsService : IMeetingEmailsService
    {
        private IEmailApiService _emailApiService;

        public MeetingEmailsService(IEmailApiService emailApiService)
        {
            _emailApiService = emailApiService;
        }

        public void Send(MeetingEntity meeting, MeetingParticipantEntity meetingParticipant)
        {
            var emailData = new SendEmailRequest()
            {
                Subject = meeting.Title,

                Message = meeting.Description,

                EmailTo = meetingParticipant.User.Email
            };

            try
            {
                _emailApiService.Send(emailData);
            } catch(Exception e)
            {

            }
        }
    }
}
