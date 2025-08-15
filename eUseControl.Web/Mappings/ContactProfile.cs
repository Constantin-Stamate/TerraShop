using AutoMapper;
using eUseControl.Domain.Entities.Contact;
using eUseControl.Web.Models.Contact;

namespace eUseControl.Web.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactCompact, ContactData>();
            CreateMap<ContactSummary, ContactInfo>();
        }
    }
}