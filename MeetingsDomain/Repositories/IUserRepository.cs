using MeetingsDomain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetingsDomain.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetUsersAsync();

        Task<UserEntity> GetUserAsync(int userId);

        Task<UserEntity> GetUserByEmailAsync(string email);

        Task<UserEntity> VerifyUserAsync(UserEntity user);

        Task<bool> AddUserAsync(UserEntity user);

        Task<bool> UpdateUserAsync(UserEntity user);

        string GetToken(UserEntity user);
    }
}
