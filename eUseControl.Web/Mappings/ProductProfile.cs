using AutoMapper;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductSummary, ProductMini>();
            CreateMap<ProductLite, ProductInfo>();
            CreateMap<Product, ProductData>();
            CreateMap<ProductData, Product>();
            CreateMap<ProductMinimal, ProductCompact>();
        }
    }
}