using MeetingsAPI.Installers.Configurations;
using MeetingsDomain.Entities;
using MeetingsDomain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MeetingsAPI.Repositories
{
    /// <summary>
    /// Service for handling manipulations for the User entity
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// The datebase context
        /// </summary>
        private readonly MeetingsDBContext _context;

        /// <summary>
        /// 
        /// </summary>
        private readonly IOptions<JWTConfiguration> _tokenConfiguration;

        public UserRepository(MeetingsDBContext context, IOptions<JWTConfiguration> tokenConfiguration)
        {
            _context = context;
            _tokenConfiguration = tokenConfiguration;
        }

        /// <summary>
        /// Method for adding User to the database context
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>Value, that indicates whether the user has been added</returns>
        public async Task<bool> AddUserAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        /// <summary>
        /// Method for generating User tokens
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>Authentication token</returns>
        public string GetToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_tokenConfiguration.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Method for getting User by it's Id from the database context
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User entity</returns>
        public async Task<UserEntity> GetUserAsync(int userId)
        {
            return await _context.Users
                    .FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                        .FirstOrDefaultAsync(user => user.Email == email);
        }

        /// <summary>
        /// Method for getting Users list from the database context
        /// </summary>
        /// <returns>User entities list</returns>
        public async Task<List<UserEntity>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        /// <summary>
        /// Method for updating the User entity inside the databes context
        /// </summary>
        /// <param name="userData">New user data</param>
        /// <returns>Value, that indicates whether the user has been updated</returns>
        public async Task<bool> UpdateUserAsync(UserEntity userData)
        {
            _context.Update(userData);
            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        /// <summary>
        /// Method detects a User entity with the provided login datain the databes context
        /// </summary>
        /// <param name="userData">Login user data</param>
        /// <returns>User entity for the provided login data if it exists</returns>
        public async Task<UserEntity> VerifyUserAsync(UserEntity userData)
        {
            return await _context.Users
                        .FirstOrDefaultAsync(user => (user.Email == userData.Email) && (user.PasswordHash == userData.PasswordHash));
        }
    }
}
