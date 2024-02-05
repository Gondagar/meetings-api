using AutoMapper;
using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Contracts.Response;
using MeetingsDomain.Entities;
using MeetingsDomain.Repositories;
using MeetingsDomain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MeetingsAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateValidationFilter]
    [ApiExceptionFilter]
    public class MeetingsController : ControllerBase
    {
        private IMeetingRepository _meetingRepository;
        private IMapper _autoMapper;
        private IUserRepository _userRepository;
        private IAvatarService _avatarService;
        private IMeetingEmailsService _emailApiService;

        public MeetingsController(IMeetingRepository meetingRepository, IUserRepository userRepository, IMapper autoMapper, IAvatarService avatarService, IMeetingEmailsService emailApiService)
        {
            _meetingRepository = meetingRepository;
            _autoMapper = autoMapper;
            _userRepository = userRepository;
            _avatarService = avatarService;
            _emailApiService = emailApiService;
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

        private string GetUserRole()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var sub = claimsIdentity.FindFirst("role")?.Value;

            try
            {
                return sub;
            }
            catch (Exception)
            {
                return "";
            }
        }

        // GET: api/meetings/all
        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            //var userRole = GetUserRole();

            //if (userRole != "admin")
            //{
            //    return Forbid();
            //}

            var meetings = await _meetingRepository.GetAll(DateTime.UtcNow);
            var meetingEmailsData = _autoMapper.Map<List<MeetingEntity>, List<MeetingEmailDetailsResponse>>(meetings);

            foreach (var meeting in meetingEmailsData)
            {
                meeting.Owner = meeting.Participants.Where(participant => participant.IsOwner).FirstOrDefault();
            }

            return Ok(meetingEmailsData);
        }

        // GET: api/meetings
        [HttpGet]
        public async Task<ActionResult> GetAllUserMeetings([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            if(endTime == null || endTime <= DateTime.UtcNow)
            {
                endTime = DateTime.UtcNow.AddDays(1000);
            }

            var userId = GetUserId();
            var meetings = await _meetingRepository.GetAll(userId, startTime, endTime);
            var destination = _autoMapper.Map<List<MeetingEntity>, List<MeetingResponse>>(meetings);

            return Ok(destination);
        }

        // GET: api/meetings/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var userId = GetUserId();
            var meeting = await _meetingRepository.GetSingle(id);

            if (meeting == null)
            {
                return NotFound("The meeting doesn't exist");
            }

            var Owner = meeting
             .Participants
             .Where(participant => participant.IsOwner)
             .FirstOrDefault();
            var result = _autoMapper.Map<MeetingEntity, MeetingDetailsResponse>(meeting);

            result.IsOwner = Owner.UserId == userId;
            result.Participants
                .Where(participant => participant.Id == Owner.Id)
                .FirstOrDefault().IsOwner = true;
            result.Participants
                .ForEach(participant => participant.UserAvatar = _avatarService.GetBase64Avatar(participant.UserAvatar));
            result.Notes
                .ForEach(note => note.Author.Avatar = _avatarService.GetBase64Avatar(note.Author.Avatar));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<MeetingDetailsResponse>> Add([FromBody] AddMeetingReqest meeting)
        {
            var newMeeting = _autoMapper.Map<AddMeetingReqest, MeetingEntity>(meeting);
            var currentUser = await _userRepository.GetUserAsync(GetUserId());
            var owner = new MeetingParticipantEntity()
            {
                User = currentUser,
                IsOwner = true
            };

            newMeeting.Participants = new List<MeetingParticipantEntity>();
            newMeeting.Participants.Add(owner);

            var updated = await _meetingRepository.Add(newMeeting);

            if (updated == 0)
            {
                throw new Exception("An error occured");
            }

            var response = new EntityCreationResponse(newMeeting.Id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var meeting = await _meetingRepository.GetSingle(id);


            if (meeting == null)
            {
                return NotFound("The meeting doesn't exist");
            }

            var isOwner = meeting
                .Participants
                .Where(participant => participant.User.Id == userId && participant.IsOwner)
                .Any();

            if (!isOwner)
            {
                return Forbid();
            }

            var updated = await _meetingRepository.Delete(meeting);

            if (updated == 0)
            {
                throw new Exception("An error occured");
            }

            return Ok();
        }

        // POST: api/meetings/5/notes
        [HttpPost("{meetingId}/notes")]
        public async Task<ActionResult> AddNote(int meetingId, [FromBody] AddMeetingNoteRequest meetingNote)
        {
            var userId = GetUserId();
            var meeting = await _meetingRepository.GetSingle(meetingId);

            if (meeting == null)
            {
                return NotFound("The meeting doesn't exist");
            }

            var meetingParticipant = meeting
                                        .Participants
                                        .Where(participant => participant.UserId == userId)
                                        .ToList();

            if (!meetingParticipant.Any())
            {
                return Forbid();
            }

            var user = await _userRepository.GetUserAsync(userId);
            var newMeetingNote = _autoMapper.Map<AddMeetingNoteRequest, MeetingNoteEntity>(meetingNote);

            newMeetingNote.Author = user;
            newMeetingNote.Meeting = meeting;

            var update = await _meetingRepository.AddNote(newMeetingNote);

            if (update == 0)
            {
                throw new Exception("An error occured");
            }

            newMeetingNote.Author.AvatarUrl = _avatarService.GetBase64Avatar(user.AvatarUrl);

            return Ok(newMeetingNote);
        }

        // DELETE: api/meetings/5/notes/5
        [HttpDelete("{meetingId}/notes/{id}")]
        public async Task<ActionResult> RemoveNote(int meetingId, int id)
        {
            var userId = GetUserId();
            var oldNote = await _meetingRepository.GetSingleNote(meetingId, id);
            var meeting = await _meetingRepository.GetSingle(meetingId);

            if (meeting == null)
            {
                return NotFound("The meeting doesn't exist");
            }

            if (oldNote == null)
            {
                return NotFound("The note doesn't exist");
            }

            var isParticipant = meeting
                .Participants
                .Where(participant => participant.User.Id == userId)
                .Any();

            if (!isParticipant)
            {
                return Forbid();
            }

            var update = await _meetingRepository.DeleteNote(oldNote);

            if (update == 0)
            {
                throw new Exception("An error occured");
            }

            return Ok();
        }

        // POST: api/meetings/5/participants
        [HttpPost("{meetingId}/participants")]
        public async Task<ActionResult> AddParticipnat(int meetingId, List<AddMeetingParticipantRequest> participants)
        {
            var userId = GetUserId();
            var meeting = await _meetingRepository.GetSingle(meetingId);

            if (meeting == null)
            {
                return NotFound("The meeting doesn't exist");
            }

            var isOwner = meeting
                .Participants
                .Where(participant => participant.User.Id == userId && participant.IsOwner)
                .Any();

            if (!isOwner)
            {
                return Forbid();
            }

            var filteredParticipants = participants.Where(participant =>
                meeting.Participants.Where(meetingParticipant =>
                    meetingParticipant.UserId == participant.UserId).Any()).ToList();

            if (filteredParticipants.Count() != 0)
            {
                ModelState.AddModelError("ParticipantDuplicated", "User already added");
                return BadRequest(ModelState);
            }

            var meetingParticipants = _autoMapper.Map<List<AddMeetingParticipantRequest>, List<MeetingParticipantEntity>>(participants);

            meetingParticipants.ForEach(participant =>
            {
                participant.MeetingId = meetingId;
            });

            var update = await _meetingRepository.AddParticipants(meetingParticipants);

            if (update == 0)
            {                
                throw new Exception("An error occured");
            }

            var updatedParticipants = await _meetingRepository.GetParticipants(meetingId);
            var meetingParticipantsResponse = _autoMapper.Map<List<MeetingParticipantEntity>, List<MeetingParticipantResponse>>(updatedParticipants);

            participants.ForEach(participant =>
            {
                var updatedParticipantData = updatedParticipants
                    .Where(updatedParticipant => updatedParticipant.UserId == participant.UserId)
                    .FirstOrDefault();

                _emailApiService.Send(meeting, updatedParticipantData);
            });

            meetingParticipantsResponse.ForEach(participant =>
                _avatarService.GetBase64Avatar(participant.UserAvatar)
            );

            return Ok(meetingParticipantsResponse);
        }

        // DELETE: api/meetings/5/participants/5
        [HttpDelete("{meetingId}/participants/{id}")]
        public async Task<ActionResult> RemoveParticipant(int meetingId, int id)
        {
            var userId = GetUserId();
            var participantEntity = await _meetingRepository.GetSingleParticipant(meetingId, id);
            var meeting = await _meetingRepository.GetSingle(meetingId);

            if (meeting == null)
            {
                return NotFound("The meeting doesn't exist");
            }

            if (participantEntity == null)
            {
                return NotFound("The meeting participant doesn't exist");
            }

            if (participantEntity.IsOwner)
            {
                ModelState.AddModelError("RemoveOwner", "It's not possible to remove the meeting owner");
                return BadRequest(ModelState);
            }

            var isOwner = meeting
                .Participants
                .Where(participant => participant.User.Id == userId && participant.IsOwner)
                .Any();

            if (!isOwner)
            {
                return Forbid();
            }

            var update = await _meetingRepository.DeleteParticipant(participantEntity);

            if (update == 0)
            {
                throw new Exception("An error occured");
            }

            return Ok();
        }
    }
}
