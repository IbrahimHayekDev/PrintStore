using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IProductService _productService;
	private readonly ICustomizableProductService _customizableProductService;

	public ProductController(IProductService productService, ICustomizableProductService customizableProductService)
	{
		_productService = productService;
		_customizableProductService = customizableProductService;
	}

	[HttpGet("GetAll")]
	[Produces("application/json", Type = typeof(Response<ListResponse<StoreProductDTO>>))]
	public async Task<IActionResult> GetStoreProducts()
	{
		var products = await _productService.GetStoreProducts();
		if (products == null)
		{
			return BadRequest(new { message = products });
		}
		return Ok(products);
	}

	[HttpGet("GetStoreVariantsByProduct")]
	[Produces("application/json", Type = typeof(Response<ListResponse<GetStoreVariantByProductIdResponse>>))]
	public async Task<IActionResult> GetStoreVariantByProductId([FromQuery] int id)
	{
		var response = await _productService.GetStoreVariantByProductId(id);
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);
	}

	[HttpGet("GetProductById")]
	public async Task<IActionResult> GetStoreProductById([FromQuery] string id)
	{
		var products = await _productService.GetStoreProductById(id);
		if (products == null)
		{
			return BadRequest(new { message = products });
		}
		return Ok(products);
	}

	[HttpGet("GetStoreProductsByCatId")]
	[Produces("application/json", Type = typeof(Response<ListResponse<PortalStoreProductDTO>>))]

	public async Task<IActionResult> GetStoreProductsByCategortyId(long categoryId)
	{
		var response = await _productService.GetStoreProductsByCategortyId(categoryId);
		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);

	}

	// Portal

	[HttpGet("GetCustomizeProductsByCategortyId")]
	[Produces("application/json", Type = typeof(Response<ListResponse<CustomizableProductDTO>>))]

	public async Task<IActionResult> getCustomieProductsByCategortyId(long categoryId)
	{
		var response = await _customizableProductService.GetCustomizeProductsByCategortyId(categoryId);

		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);

	}

	[HttpGet("GetCustomizeProducDetailstbyId")]
	[Produces("application/json", Type = typeof(Response<ListResponse<CustomizableProductDTO>>))]
	public async Task<IActionResult> getCustomizeProducDetailstbyId(int id)
	{
		var response = await _customizableProductService.GetCustomizeProducDetailstbyId(id);

		if (response == null)
		{
			return BadRequest(new { message = response });
		}
		return Ok(response);
	}
}
