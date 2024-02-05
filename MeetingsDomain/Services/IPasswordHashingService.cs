using MeetingsDomain.Entities;

namespace MeetingsDomain.Services
{
    public interface IPasswordHashingService
    {
        string GetPasswordHash(string Password);

        bool ValidatePasswordHash(string Password, UserEntity User);
    }
}
