using AutoMapper;
using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Contracts.Response;
using MeetingsDomain.Entities;

namespace MeetingsAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<UserEntity, UserResponse>()
                .ForMember(
                    distination => distination.Avatar,
                    opt => opt.MapFrom(src => src.AvatarUrl)
                );
            this.CreateMap<MeetingParticipantEntity, MeetingParticipantResponse>()
                .ForMember(
                    distination => distination.UserAvatar,
                    opt => opt.MapFrom(src => src.User.AvatarUrl)
                );
            this.CreateMap<MeetingParticipantEntity, MeetingEmailParticipantResponse>();
            this.CreateMap<AddMeetingParticipantRequest, MeetingParticipantEntity>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                    )
                );
            this.CreateMap<AddUserRequest, UserEntity>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                    )
                );
        }
    }
}
