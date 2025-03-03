using PrintStoreApi.Core.Entities.Product;

namespace PrintStoreApi.Models.Products.Customizable;

public class CustomizableVariantDTO
{
	public int Id { get; set; }

	public long PrintfulId { get; set; }

	public int BaseProductId { get; set; }

	public long PrintfulProductId { get; set; }

	public string VariantName { get; set; }

	public string Color { get; set; }

	public string? ColorCode1 { get; set; }

	public string? ColorCode2 { get; set; }

	public string ImageUrl { get; set; }

	public decimal Price { get; set; }

	public string Size { get; set; }

	public int InStock { get; set; }

	// Navigation properties
	public CustomizableProductDTO? CustomizableProduct { get; set; }
	public virtual List<ProductMaterialDTO>? ProductMaterials { get; set; }
	public virtual List<AvailableRegionDTO>? AvailableRegions { get; set; }


}
