using PrintStoreApi.Core.Entities.Product;
using System.ComponentModel.DataAnnotations;

namespace PrintStoreApi.Models.Products.Customizable;

public class CustomizableProductDTO
{
	public int Id { get; set; }

	public long PrintfulId { get; set; }

	public long CategoryId { get; set; }

	public string ProductType { get; set; }

	public string ProductTypeName { get; set; }

	public string ProductDescription { get; set; }

	public string Title { get; set; }

	public string? Brand { get; set; }

	public string ImageUrl { get; set; }

	public string Model { get; set; }

	public int VariantCount { get; set; }

	public string Currency { get; set; }

	public int IsDiscontinued { get; set; }

	public decimal? AverageFulfilmentTime { get; set; }

	public string? OriginCountry { get; set; }


	public decimal MinPrice { get; set; }

	public decimal MaxPrice { get; set; }

	// Navigation properties
	public ICollection<CustomizableVariantDTO>? CustomizableVarints { get; set; } = new List<CustomizableVariantDTO>();
	public virtual ProductCategory? ProductCategory { get; set; }
}
