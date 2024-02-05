namespace MeetingsDomain.Contracts.Request
{
    public class SendEmailRequest
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public string EmailTo { get; set; }
    }
}
