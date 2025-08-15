using AutoMapper;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;
using eUseControl.Web.Models.User;

namespace eUseControl.Web.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserLogin, ULoginData>();
            CreateMap<UserLite, UserInfo>();
            CreateMap<UserSummary, UserCompact>();
            CreateMap<UserMinimal, UserCompact>();
            CreateMap<UserRegister, URegisterData>();
        }
    }
}