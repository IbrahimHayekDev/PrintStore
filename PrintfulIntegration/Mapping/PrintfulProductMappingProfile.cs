using AutoMapper;

using PrintfulIntegration.Models.common;
using PrintfulIntegration.Models.PrintfulResponse;
using PrintfulIntegration.Models.Products;

namespace PrintfulIntegration.Mapping;

public class PrintfulProductMappingProfile : Profile
{
	public PrintfulProductMappingProfile()
	{
		CreateMap<PrintfulPaging, Paging>();
	}
}
