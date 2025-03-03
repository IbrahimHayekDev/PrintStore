using AutoMapper;

using Microsoft.Extensions.Options;

using PrintfulIntegration.configuration;
using PrintfulIntegration.Core.Interfaces.Services;
using PrintfulIntegration.Models.common;
using PrintfulIntegration.Models.PrintfulResponse;
using PrintfulIntegration.Models.Products;
using PrintfulIntegration.Models.Products.ProductCatalog;

using System.Text.Json;

namespace PrintfulIntegration.Services;

public class PrintfulCategotyService: IPrintfulCategotyService
{
	private readonly string _apiKey;
	private readonly IMapper _mapper;
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl;

	public PrintfulCategotyService(IOptions<PrintfulConfig> config, IMapper mapper, HttpClient httpClient)
	{
		_apiKey = config.Value.ApiKey;
		_mapper = mapper;
		_baseUrl = config.Value.BaseUrl;
		_httpClient = httpClient;
		_httpClient.BaseAddress = new Uri(_baseUrl);
		_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
	}

	public async Task<Response<PrintfulCategoryResponse>> GetPrintfulCategories()
	{
		var response = new Response<PrintfulCategoryResponse>();

		var categoryResponse = new PrintfulResponse<PrintfulCategoryResponse>();


		var apiResponse = await _httpClient.GetAsync("/categories");
		apiResponse.EnsureSuccessStatusCode();
		if (!apiResponse.IsSuccessStatusCode)
		{
			response.Error.Errors.Add($"External api returns error {apiResponse.StatusCode}");
			return response;
		}
		var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
		categoryResponse = JsonSerializer.Deserialize< PrintfulResponse < PrintfulCategoryResponse>> (jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		if (categoryResponse == null)
		{
			response.Error.Errors.Add("Invalid api response format");
			return response;
		}
		if (categoryResponse.Error != null)
		{
			response.Error.Errors.Add($"Reason: {categoryResponse.Error.Reason} | Message:  {categoryResponse.Error.Message}");
			return response;
		}
		response.Data = categoryResponse.Result;
		return response;
	}
}
