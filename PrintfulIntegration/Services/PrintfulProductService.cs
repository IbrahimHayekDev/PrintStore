using AutoMapper;

using Microsoft.Extensions.Options;

using PrintfulIntegration.configuration;
using PrintfulIntegration.Core.Interfaces.Services;
using PrintfulIntegration.Models.common;
using PrintfulIntegration.Models.PrintfulResponse;
using PrintfulIntegration.Models.Products;
using PrintfulIntegration.Models.Products.ProductCatalog;


//using RestSharp;

using System.Text.Json;

namespace PrintfulIntegration.Services;

public class PrintfulProductService : IPrintfulProductService
{
	//private RestClient _client;
	private readonly string _apiKey;
	private readonly IMapper _mapper;
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl;

	public PrintfulProductService(IOptions<PrintfulConfig> config, IMapper mapper, HttpClient httpClient)
	{
		//_client = new RestClient("https://api.printful.com");
		_apiKey = config.Value.ApiKey;
		_mapper = mapper;
		_httpClient = httpClient;
		_baseUrl = config.Value.BaseUrl;
		_httpClient.BaseAddress = new Uri(_baseUrl);
		_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
	}

	public async Task<Response<ResponseList<PrintfulProduct>>> GetStoreProductsAsync()
	{
		var response = new Response<ResponseList<PrintfulProduct>>();
		var listingResponse = new ResponseList<PrintfulProduct>();
		var productResponse = new PrintfulResponse<List<PrintfulProduct>>();
		var apiResponse = await _httpClient.GetAsync( "/store/products");
		apiResponse.EnsureSuccessStatusCode();
		if (!apiResponse.IsSuccessStatusCode)
		{
			response.Error.Errors.Add($"External api returns error {apiResponse.StatusCode}");
			return response;
		}
		var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
		productResponse = JsonSerializer.Deserialize<PrintfulResponse<List<PrintfulProduct>>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		if (productResponse == null)
		{
			response.Error.Errors.Add("Invalid api response format");
			return response;
		}
		if (productResponse.Error != null)
		{
			response.Error.Errors.Add($"Reason: {productResponse.Error.Reason} | Message:  {productResponse.Error.Message}");
			return response;
		}
		var mappedPaging = _mapper.Map<Paging>(productResponse.Paging ?? new PrintfulPaging());
		listingResponse.Items = productResponse.Result;
		listingResponse.Paging = mappedPaging;
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<PrintfulProductById>> GetStoreProductByIdAsync(string Id)
	{
		var response = new Response<PrintfulProductById>();

		if (Id == null)
		{
			response.Error.Errors.Add($"Product Id is required");
			return response;
		}
		var productResponse = new PrintfulResponse<PrintfulProductById>();
		var apiResponse = await _httpClient.GetAsync($"/store/products/{Id}");
		apiResponse.EnsureSuccessStatusCode();
		if (!apiResponse.IsSuccessStatusCode)
		{
			response.Error.Errors.Add($"External api returns error {apiResponse.StatusCode}");
			return response;
		}
		var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
		productResponse = JsonSerializer.Deserialize<PrintfulResponse<PrintfulProductById>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		if (productResponse == null)
		{
			response.Error.Errors.Add("Invalid api response format");
			return response;
		}
		if (productResponse.Error != null)
		{
			response.Error.Errors.Add($"Reason: {productResponse.Error.Reason} | Message:  {productResponse.Error.Message}");
			return response;
		}
		response.Data = productResponse.Result;
		return response;
	}

	public async Task<Response<ResponseList<PrintfulCatalogProduct>>> GetAllBaseProducts()
	{
		var response = new Response<ResponseList<PrintfulCatalogProduct>>();
		var listingResponse = new ResponseList<PrintfulCatalogProduct>();
		var products = new PrintfulResponse<List<PrintfulCatalogProduct>>();
		var apiResponse = await _httpClient.GetAsync("/products");
		apiResponse.EnsureSuccessStatusCode();
		if (!apiResponse.IsSuccessStatusCode)
		{
			response.Error.Errors.Add($"External api returns error {apiResponse.StatusCode}");
			return response;
		}
		var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
		products = JsonSerializer.Deserialize<PrintfulResponse<List<PrintfulCatalogProduct>>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		listingResponse.Items = products.Result;
		response.Data = listingResponse;
		return response;

	}
	public async Task<Response<PrintfulProductCatalogById>> GetProductByIdAsync(long Id)
	{
		var response = new Response<PrintfulProductCatalogById>();

		if (Id == null)
		{
			response.Error.Errors.Add($"Product Id is required");
			return response;
		}
		var productResponse = new PrintfulResponse<PrintfulProductCatalogById>();
		var apiResponse = await _httpClient.GetAsync($"/products/{Id}");
		//apiResponse.IsSuccessStatusCode();
		if (!apiResponse.IsSuccessStatusCode)
		{
			response.Error.Errors.Add($"External api returns error {apiResponse.StatusCode}");
			return response;
		}
		var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
		productResponse = JsonSerializer.Deserialize<PrintfulResponse<PrintfulProductCatalogById>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		if (productResponse == null)
		{
			response.Error.Errors.Add("Invalid api response format");
			return response;
		}
		if (productResponse.Error != null)
		{
			response.Error.Errors.Add($"Reason: {productResponse.Error.Reason} | Message:  {productResponse.Error.Message}");
			return response;
		}
		response.Data = productResponse.Result;
		return response;
	}

	public async Task<Response<ProductSizeGuide>> GetSizeGuideByProductId(long ProductId)
	{
		var response = new Response<ProductSizeGuide>();
		if (ProductId == null)
		{
			response.Error.Errors.Add($"Product Id is required");
			return response;
		}
		var productResponse = new PrintfulResponse<ProductSizeGuide>();
		var apiResponse = await _httpClient.GetAsync($"/products/{ProductId}/sizes?unit=inches,cm");
		apiResponse.EnsureSuccessStatusCode();
		if (!apiResponse.IsSuccessStatusCode)
		{
			response.Error.Errors.Add($"External api returns error {apiResponse.StatusCode}");
			return response;
		}
		var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
		productResponse = JsonSerializer.Deserialize<PrintfulResponse<ProductSizeGuide>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		if (productResponse == null)
		{
			response.Error.Errors.Add("Invalid api response format");
			return response;
		}
		if (productResponse.Error != null)
		{
			response.Error.Errors.Add($"Reason: {productResponse.Error.Reason} | Message:  {productResponse.Error.Message}");
			return response;
		}
		response.Data = productResponse.Result;
		return response;
	}

}
