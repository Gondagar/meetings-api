using MeetingsDomain.Contracts.Request;

namespace MeetingsDomain.Services
{
    public interface IEmailApiService
    {
        void Send(SendEmailRequest email);
    }
}
