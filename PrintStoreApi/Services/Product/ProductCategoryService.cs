using AutoMapper;

using PrintfulIntegration.Core.Interfaces.Services;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Portal;


namespace PrintStoreApi.Services.Product;

public class ProductCategoryService: IProductCategoryService
{
	private readonly IPrintfulCategotyService _printfulCategotyService;
	private readonly IProductCategoryRepository _productCategoryRepository;
	private readonly IMapper _mapper;

	public ProductCategoryService(IPrintfulCategotyService printfulCategotyService, IProductCategoryRepository productCategoryRepository, IMapper mapper)
	{
		_printfulCategotyService = printfulCategotyService;
		_productCategoryRepository = productCategoryRepository;
		_mapper = mapper;
	}

	public async Task<Response<ListResponse<ProductCategoryDTO>>> GetAllCategoriesAsync()
	{
		var response = new Response<ListResponse<ProductCategoryDTO>>();
		var listingResponse = new ListResponse<ProductCategoryDTO>();
		var result = await _productCategoryRepository.GetNestedCategoriesAsync();
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}
		listingResponse.Items = result;
		listingResponse.TotalCount = result.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<ListResponse<PortalCategoryDTO>>> GetCategoriesWithProducts()
	{
		var response = new Response<ListResponse<PortalCategoryDTO>>();
		var listingResponse = new ListResponse<PortalCategoryDTO>();
		var result = await _productCategoryRepository.GetCategoriesWithProducts();
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}
		var categories = _mapper.Map<List<PortalCategoryDTO>>(result);

		listingResponse.Items = categories;
		listingResponse.TotalCount = categories.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<ListResponse<PortalCategoryDTO>>> GetCategoriesWithCustomizableProducts()
	{
		var response = new Response<ListResponse<PortalCategoryDTO>>();
		var listingResponse = new ListResponse<PortalCategoryDTO>();
		var result = await _productCategoryRepository.GetCategoriesWithCustomizableProducts();
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching categories");
			return response;
		}
		var categories = _mapper.Map<List<PortalCategoryDTO>>(result);

		listingResponse.Items = categories;
		listingResponse.TotalCount = categories.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<ListResponse<PortalCategoryDTO>>> GetPortalCategoriesById(int categoryId)
	{
		var response = new Response<ListResponse<PortalCategoryDTO>>();
		var listingResponse = new ListResponse<PortalCategoryDTO>();
		var result = await _productCategoryRepository.GetPortalCategoriesById(categoryId);
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}
		listingResponse.Items = result;
		listingResponse.TotalCount = result.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<ListResponse<PortalMainCategoryDTO>>> GetPortalMainCategories()
	{
		var response = new Response<ListResponse<PortalMainCategoryDTO>>();
		var listingResponse = new ListResponse<PortalMainCategoryDTO>();
		var result = await _productCategoryRepository.GetPortalMainCategories();
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}
		listingResponse.Items = result;
		listingResponse.TotalCount = result.Count();
		response.Data = listingResponse;
		return response;
	}
}


