using AutoMapper;
using eUseControl.Domain.Entities.Payment;
using eUseControl.Web.Models.Payment;

namespace eUseControl.Web.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionCompact, TransactionData>();
        }
    }
}