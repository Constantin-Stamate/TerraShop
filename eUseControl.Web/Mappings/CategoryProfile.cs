using AutoMapper;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryData, ProductCategory>();
        }
    }
}