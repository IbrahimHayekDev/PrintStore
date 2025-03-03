using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Portal;
using PrintStoreApi.Services.Product;

namespace PrintStoreApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly IProductCategoryService _categoryService;
	public CategoryController(IProductCategoryService categoryService)
	{
		_categoryService = categoryService;
	}

	[HttpGet("GetAll")]
	[Produces("application/json", Type = typeof(Response<ListResponse<ProductCategoryDTO>>))]

	public async Task<IActionResult> GetAllCategories()
	{
		var response = await _categoryService.GetAllCategoriesAsync();
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);


	}

	// Portal

	[HttpGet("GetCategoriesWithCustomizableProducts")]
	[Produces("application/json", Type = typeof(Response<ListResponse<PortalCategoryDTO>>))]

	public async Task<IActionResult> getCategoriesWithCustomizableProducts()
	{

		var response = await _categoryService.GetCategoriesWithCustomizableProducts();
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);

	}


	[HttpGet("GetPortalCategoriesById")]
	[Produces("application/json", Type = typeof(Response<ListResponse<PortalCategoryDTO>>))]

	public async Task<IActionResult> getPortalCategoriesById([FromQuery] int categoryId)
	{
		var response = await _categoryService.GetPortalCategoriesById(categoryId);
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);

	}

	[HttpGet("GetPortalMainCategories")]
	[Produces("application/json", Type = typeof(Response<ListResponse<PortalCategoryDTO>>))]

	public async Task<IActionResult> getPortalMainCategories()
	{
		var response = await _categoryService.GetPortalMainCategories();
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);
	}
}
