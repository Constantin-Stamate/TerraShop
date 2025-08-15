using AutoMapper;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Web.Models.Profile;

namespace eUseControl.Web.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<ProfileData, ProfileMini>();
            CreateMap<ProfileData, ProfileCompact>();
            CreateMap<ProfileCompact, ProfileData>();
        }
    }
}