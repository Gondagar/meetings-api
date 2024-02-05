using AutoMapper;
using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Contracts.Response;
using MeetingsDomain.Entities;
using MeetingsDomain.Repositories;
using MeetingsDomain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeetingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateValidationFilter]
    [ApiExceptionFilter]
    public class AuthController : ControllerBase
    {
        private IMapper _autoMapper;
        private IUserRepository _userRepositry;
        private IAvatarService _avatarService;
        private IPasswordHashingService _passwordHashingService;

        public AuthController(
            IMapper autoMapper,
            IUserRepository userService,
            IAvatarService avatarService,
            IPasswordHashingService passwordHashingService
        )
        {
            _autoMapper = autoMapper;
            _userRepositry = userService;
            _avatarService = avatarService;
            _passwordHashingService = passwordHashingService;
        }

        // POST api/auth
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> SignUp([FromForm] AddUserRequest userReguestData)
        {
            var existingUser = await _userRepositry.GetUserByEmailAsync(userReguestData.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("EmailNotUnique", "Email already in use");
                return BadRequest(ModelState);
            }

            var userData = _autoMapper.Map<AddUserRequest, UserEntity>(userReguestData);
            var avatarPath = _avatarService.SaveAvatar(null);

            userData.AvatarUrl = avatarPath;
            userData.PasswordHash = _passwordHashingService.GetPasswordHash(userReguestData.Password);
            await _userRepositry.AddUserAsync(userData);

            var userToken = _userRepositry.GetToken(userData);
            var reposne = new AuthenticationResponse()
            {
                AccessToken = userToken
            };

            return Ok(reposne);
        }

        // POST api/auth
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<IUserRepository>> SignIn([FromForm] LoginUserRequest userLoginData)
        {
            var userByEmail = await _userRepositry.GetUserByEmailAsync(userLoginData.Email);

            if (userByEmail == null)
            {
                ModelState.AddModelError("InvalidCredentials", "Invalid login or password");
                return BadRequest(ModelState);
            }

            var isRightPassword = _passwordHashingService.ValidatePasswordHash(userLoginData.Password, userByEmail);

            if (isRightPassword)
            {
                var userToken = _userRepositry.GetToken(userByEmail);
                var reposne = new AuthenticationResponse()
                {
                    AccessToken = userToken
                };

                return Ok(reposne);
            }

            ModelState.AddModelError("InvalidCredentials", "Invalid login or password");
            return BadRequest(ModelState);
        }
    }
}