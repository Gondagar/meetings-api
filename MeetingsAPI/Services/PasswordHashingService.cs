using MeetingsAPI.Installers.Configurations;
using MeetingsDomain.Entities;
using MeetingsDomain.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace MeetingsAPI.Services
{
    public class PasswordHashingService : IPasswordHashingService
    {
        private readonly IOptions<PasswordConfiguration> _passwordConfiguration;

        public PasswordHashingService(IOptions<PasswordConfiguration> passwordConfiguration)
        {
            _passwordConfiguration = passwordConfiguration;
        }

        /// <summary>
        /// Method sets the password hash
        /// </summary>
        /// <param name="Password"></param>
        /// <returns>Hashed password</returns>
        public string GetPasswordHash(string Password)
        {
            string passwordSalt = _passwordConfiguration.Value.Salt;
            byte[] passwordSaltBytesArray = Encoding.ASCII.GetBytes(passwordSalt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: Password,
                   salt: passwordSaltBytesArray,
                   prf: KeyDerivationPrf.HMACSHA1,
                   iterationCount: 10000,
                   numBytesRequested: 256 / 8));

            return hashed;
        }

        /// <summary>
        /// Validates the incomming password by existing User entity
        /// </summary>
        /// <param name="Password">Incomming password</param>
        /// <param name="User">Existing User entity</param>
        /// <returns>Indicator whether the password hash is valid</returns>
        public bool ValidatePasswordHash(string Password, UserEntity User)
        {
            return GetPasswordHash(Password) == User.PasswordHash;
        }
    }
}
