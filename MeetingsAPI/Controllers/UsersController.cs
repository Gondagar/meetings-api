using AutoMapper;
using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Contracts.Response;
using MeetingsDomain.Entities;
using MeetingsDomain.Repositories;
using MeetingsDomain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeetingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ModelStateValidationFilter]
    [ApiExceptionFilter]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepositry;
        private IMapper _autoMapper;
        private IAvatarService _avatarService;

        public UsersController(IUserRepository userService, IMapper autoMapper, IAvatarService avatarService)
        {
            _userRepositry = userService;
            _autoMapper = autoMapper;
            _avatarService = avatarService;
        }
        private int GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var sub = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                return Int32.Parse(sub);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUserData()
        {
            var user = await _userRepositry.GetUserAsync(GetUserId());

            if (user == null)
            {
                return Unauthorized();
            }

            var userDisplayModel = _autoMapper.Map<UserEntity, UserResponse>(user);
            userDisplayModel.Avatar = _avatarService.GetBase64Avatar(userDisplayModel.Avatar);

            return new ObjectResult(userDisplayModel);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> Get()
        {
            var users = await _userRepositry.GetUsersAsync();
            var userDisplayModels = _autoMapper.Map<List<UserEntity>, List<UserResponse>>(users);

            userDisplayModels.ForEach(user =>
            {
                user.Avatar = _avatarService.GetBase64Avatar(user.Avatar);
            });

            return new ObjectResult(userDisplayModels);
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> Get(int id)
        {
            var user = await _userRepositry.GetUserAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userDisplayModel = _autoMapper.Map<UserEntity, UserResponse>(user);
            userDisplayModel.Avatar = _avatarService.GetBase64Avatar(userDisplayModel.Avatar);

            return new ObjectResult(userDisplayModel);
        }

        // PUT api/users/avatar
        [Route("avatar")]
        [HttpPut]
        public async Task<ActionResult<UserResponse>> UpdateAvatar(IFormFile avatar)
        {
            var oldUserData = await _userRepositry.GetUserAsync(GetUserId());

            if (oldUserData == null)
            {
                return Unauthorized();
            }

            var avatarPath = _avatarService.SaveAvatar(avatar);

            oldUserData.AvatarUrl = avatarPath;
            var userAdded = await _userRepositry.UpdateUserAsync(oldUserData);

            if (userAdded)
            {
                return Ok();
            }

            throw new Exception();
        }
    }
}