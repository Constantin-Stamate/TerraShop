using AutoMapper;
using eUseControl.Domain.Entities.Review;
using eUseControl.Web.Models.Review;

namespace eUseControl.Web.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ReviewData, ReviewMini>();
            CreateMap<ReviewSummary, ReviewInfo>();
            CreateMap<ReviewData, ReviewCompact>();
            CreateMap<ReviewCompact, ReviewData>();
        }
    }
}