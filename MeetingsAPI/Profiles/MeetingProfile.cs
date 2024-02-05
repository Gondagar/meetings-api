using AutoMapper;
using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Contracts.Response;
using MeetingsDomain.Entities;

namespace MeetingsAPI.Profiles
{
    public class MeetingProfile : Profile
    {
        public MeetingProfile()
        {
            this.CreateMap<AddMeetingReqest, MeetingEntity>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                        )
                    );
            this.CreateMap<AddMeetingNoteRequest, MeetingNoteEntity>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                        )
                    );
            this.CreateMap<MeetingEntity, MeetingResponse>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                        )
                    );
            this.CreateMap<MeetingEntity, MeetingEmailDetailsResponse>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                        )
                    );
            this.CreateMap<MeetingNoteEntity, MeetingNoteResponse>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                        )
                    );
            this.CreateMap<MeetingEntity, MeetingDetailsResponse>()
                .ForAllMembers(
                    opt => opt.Condition(
                        (source, dest, sourceMember, destMember) => (sourceMember != null)
                        )
                    );

        }
    }
}
