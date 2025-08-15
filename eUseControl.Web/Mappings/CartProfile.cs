using AutoMapper;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Web.Models.Cart;

namespace eUseControl.Web.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartData, CartCompact>();
        }
    }
}