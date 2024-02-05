namespace MeetingsDomain.Contracts.Response
{
    public class MeetingParticipantResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public string UserFirstName { get; set; }
        
        public string UserLastName { get; set; }

        public string UserAvatar { get; set; }

        public string UserEmail { get; set; }

        public bool IsOwner { get; set; }
    }
}
