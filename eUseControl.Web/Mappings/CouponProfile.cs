using AutoMapper;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Web.Models.Cart;

namespace eUseControl.Web.Mappings
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<CouponData, CouponCompact>();
            CreateMap<CouponCompact, CouponData>();
        }
    }
}