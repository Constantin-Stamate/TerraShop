using AutoMapper;
using eUseControl.Domain.Entities.Chat;
using eUseControl.Web.Models.Chat;

namespace eUseControl.Web.Mappings
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatData, ChatCompact>();
        }
    }
}