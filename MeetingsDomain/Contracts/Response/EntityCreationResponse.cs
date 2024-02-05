namespace MeetingsDomain.Contracts.Response
{
    public class EntityCreationResponse
    {
        public int Id { get; set; }

        public EntityCreationResponse(int entityId)
        {
            Id = entityId;
        }
    }
}
