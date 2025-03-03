
using PrintfulIntegration.Models.Products.ProductCatalog;
using PrintfulIntegration.Models.Products;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Products;

using System.Drawing;
using PrintStoreApi.Models.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using SendGrid;
using AutoMapper;
using PrintStoreApi.Repositories.Products;
using PrintStoreApi.Models.Products.Portal;


namespace PrintStoreApi.Services.Product;

public class ProductService: IProductService
{
	private readonly IStoreProductRepository _storeProductRepository;
	private readonly IStoreVariantRepository _storeVariantRepository;
	private readonly IMapper _mapper;
	public ProductService(
		IStoreProductRepository storeProductRepository,
		IStoreVariantRepository storeVariantRepository,
		IMapper mapper)
	{
		_storeProductRepository = storeProductRepository;
		_storeVariantRepository	= storeVariantRepository;
		_mapper = mapper;
	}

	
	public async Task<Response<ListResponse<StoreProductDTO>>> GetStoreProducts()
	{
		var response = new Response<ListResponse<StoreProductDTO>>();
		var listingResponse = new ListResponse<StoreProductDTO>();
		var products = new List<StoreProductDTO>();
		var productsDB = await _storeProductRepository.GetAllAsync();

		if (productsDB == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}

		listingResponse.Items = _mapper.Map<List<StoreProductDTO>>(productsDB);
		listingResponse.TotalCount = products.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<ListResponse<GetStoreVariantByProductIdResponse>>> GetStoreVariantByProductId(int id)
	{
		var response = new Response<ListResponse<GetStoreVariantByProductIdResponse>>();
		var listingResponse = new ListResponse<GetStoreVariantByProductIdResponse>();
		var variants = await _storeVariantRepository.GetVariantsByProductId(id);

		if (variants == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}

		var variantsDTO = _mapper.Map<List<StoreVariantDTO>>(variants);
		foreach (var item in variantsDTO)
		{
			item.fileUrl = item.Files.Count() > 0 ? item.Files[0].ThumbnailUrl : null;
			item.CategoryName = item.ProductCategory != null ? item.ProductCategory.Title  : null;
		}
		var responseResult = _mapper.Map<List<GetStoreVariantByProductIdResponse>>(variantsDTO);
		listingResponse.Items = responseResult;
		listingResponse.TotalCount = responseResult.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<StoreProductDTO>> GetStoreProductById(string id)
	{
		var response = new Response<StoreProductDTO>();
		var product = new StoreProductDTO();
		var productDB = await _storeProductRepository.GetByIdAsync(int.Parse(id));
		if(productDB == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}
		product = new StoreProductDTO
		{
			PrintfulId = productDB.PrintfulId,
			BaseProductId = productDB.BaseProductId,
			ExternalID = productDB.ExternalID,
			ProductName = productDB.ProductName,
			VariantCount = productDB.VariantCount,
			SyncedVariantCount = productDB.SyncedVariantCount,
			ThumbnailUrl = productDB.ThumbnailUrl,
			IsIgnored = productDB.IsIgnored,
			Id = productDB.Id,
		};
		var storeVariantsDB = (await _storeVariantRepository.GetStoreVariantsByProductID(id)).Select(variantDB => new StoreVariantDTO
		{
			PrintfulId = variantDB.PrintfulId,
			BaseVariantId = variantDB.BaseVariantId,
			ExternalID = variantDB.ExternalID,
			PrintfulStoreProductId = variantDB.PrintfulStoreProductId,
			AvailabilityStatus = variantDB.AvailabilityStatus,
			Color = variantDB.Color,
			Size = variantDB.Size,
			Currency = variantDB.Currency,
			SKU = variantDB.SKU,
			VariantName = variantDB.VariantName,
			RetailPrice = variantDB.RetailPrice,
			CategoryId = variantDB.CategoryId,
			IsIgnored = variantDB.IsIgnored,
		});
		product.StoreVariants = storeVariantsDB.ToList();
		response.Data = product;
		return response;
	}

	public async Task<Response<ListResponse<PortalStoreProductDTO>>> GetStoreProductsByCategortyId(long categoryId)
	{
		var response = new Response<ListResponse<PortalStoreProductDTO>>();
		var listingResponse = new ListResponse<PortalStoreProductDTO>();
		var result = await _storeProductRepository.GetStoreProductsByCategortyId(categoryId);
		if (result == null)
		{
			response.Error.Errors.Add("Error fetching products");
			return response;
		}
		//var products = _mapper.Map<List<PortalStoreProductDTO>>(result);

		listingResponse.Items = result;
		listingResponse.TotalCount = result.Count();
		response.Data = listingResponse;
		return response;
	}
}
