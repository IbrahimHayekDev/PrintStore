using AutoMapper;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Configuration;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<StoreProduct, StoreProductDTO>();
		CreateMap<StoreVariant, StoreVariantDTO>();
		CreateMap<VariantFile, VariantFileDTO>();
		CreateMap<ProductCategory, ProductCategoryDTO>();
		CreateMap<StoreVariantDTO, GetStoreVariantByProductIdResponse>();
		CreateMap<ProductCategory, PortalCategoryDTO>().ForMember(dest => dest.PId, opt => opt.MapFrom(src => src.PrintfulId)); ;
		CreateMap<StoreProduct, PortalStoreProductDTO>();

	}
}
