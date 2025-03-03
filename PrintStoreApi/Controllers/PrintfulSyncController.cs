using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Customizable;

using System.Reflection;

namespace PrintStoreApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PrintfulSyncController : ControllerBase
{

	private readonly IPrintfulSyncService _printfulSyncService;
	private readonly IProductCategoryService _categoryService;

	public PrintfulSyncController(IPrintfulSyncService printfulSyncService, IProductCategoryService categoryService)
	{
		_printfulSyncService = printfulSyncService;
		_categoryService = categoryService;
	}

	[HttpGet("FetchProducts")]
	[Produces("application/json", Type = typeof(Response<ListResponse<SyncProductsResponse>>))]

	public async Task<IActionResult> getProductsToSyncAsync()
	{
		try
		{
			var response = await _printfulSyncService.GetProductsToSyncAsync();
			if (response.IsSuccessful)
			{
				return StatusCode(200, response);
			}
			else
			{
				return StatusCode(400, response);
			}
		}
		catch
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
		}

	}

	[HttpPost("SyncProduct")]
	[Produces("application/json", Type = typeof(Response<bool>))]

	public async Task<IActionResult> SyncSingleProduct([FromBody] SyncProductsResponse product)
	{
		var response = await _printfulSyncService.SyncSingleProductAsync(product);
		try
		{
			if (response.IsSuccessful)
			{
				return StatusCode(200, response);
			}
			else
			{
				return StatusCode(400, response);
			}
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
		}
	}

	[HttpGet("FetchCategories")]
	[Produces("application/json", Type = typeof(Response<ListResponse<ProductCategorySyncDTO>>))]
	public async Task<IActionResult> GetCategoriesToSync()
	{
		try
		{
			var response = await _printfulSyncService.GetCategoriesToSync();
			if (response.IsSuccessful)
			{
				return StatusCode(200, response);
			}
			else
			{
				return StatusCode(400, response);
			}
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
		}
	}

	[HttpPost("SyncCategories")]
	[Produces("application/json", Type = typeof(Response<bool>))]
	public async Task<IActionResult> SyncCategories()
	{
		try
		{
			var response = await _printfulSyncService.SyncCategoriesWithDB();
			if (response.IsSuccessful)
			{
				return StatusCode(200, response);
			}
			else
			{
				return StatusCode(400, response);
			}
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
		}
	}

	[HttpPost("SyncCategory")]
	[Produces("application/json", Type = typeof(Response<bool>))]
	public async Task<IActionResult> SyncCategory([FromBody] ProductCategorySyncDTO category)
	{
		try
		{
			var response = await _printfulSyncService.SyncSingleCategory(category);
			if (response.IsSuccessful)
			{
				return StatusCode(200, response);
			}
			else
			{
				return StatusCode(400, response);
			}
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
		}
	}

	[HttpGet("SyncCustomizableProducts")]
	[Produces("application/json", Type = typeof(Response<string>))]

	public async Task<IActionResult> syncCustomizableProducts()
	{

		var response = await _printfulSyncService.SyncCustomizableProducts();
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);
	}


	[HttpGet("ProductSizeGuideByProductId")]
	[Produces("application/json", Type = typeof(Response<ProductSizeGuideDTO>))]

	public async Task<IActionResult> productSizeGuideByProductId(long pId)
	{

		var response = await _printfulSyncService.ProductSizeGuideByPIdAsync(pId);
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);
	}
}
