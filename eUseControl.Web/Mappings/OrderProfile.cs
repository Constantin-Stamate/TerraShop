using AutoMapper;
using eUseControl.Domain.Entities.Order;
using eUseControl.Web.Models.Order;

namespace eUseControl.Web.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderCompact, OrderData>();
            CreateMap<OrderLite, OrderInfo>();
            CreateMap<OrderMinimal, OrderMini>();
        }
    }
}