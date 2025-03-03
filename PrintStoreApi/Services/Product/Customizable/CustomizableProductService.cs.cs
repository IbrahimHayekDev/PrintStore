using AutoMapper;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.Products.Portal;
using PrintStoreApi.Repositories.Products;

namespace PrintStoreApi.Services.Product.Customizable;

public class CustomizableProductService: ICustomizableProductService
{
	private readonly ICustomizableProductRepository _customizableProductRepository;
	private readonly IMapper _mapper;
	public CustomizableProductService(
		ICustomizableProductRepository customizableProductRepository,
		IMapper mapper)
	{
		_customizableProductRepository = customizableProductRepository;
		_mapper = mapper;
	}

	public async Task<Response<PortaleCategoryDetailsResponseDTO>> GetCustomizeProductsByCategortyId(long categoryId)
	{
		var response = new Response<PortaleCategoryDetailsResponseDTO>();
		var result = await _customizableProductRepository.GetCustomizeProductsByCategortyId(categoryId);
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}

		response.Data = result;
		return response;
	}

	public async Task<Response<CustomizableProductDTO>> GetCustomizeProducDetailstbyId(int id)
	{
		var response = new Response<CustomizableProductDTO>();
		var result = await _customizableProductRepository.GetCustomizeProducDetailstbyId(id);
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}


		response.Data = result;
		return response;

	}
}
