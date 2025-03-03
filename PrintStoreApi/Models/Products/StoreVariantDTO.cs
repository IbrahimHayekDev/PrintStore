using PrintStoreApi.Core.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PrintStoreApi.Models.Products;

public class StoreVariantDTO
{
	public int Id { get; set; }

	public long PrintfulId { get; set; }

	public int BaseProductId { get; set; }

	public int StoreProductId { get; set; }

	public int BaseVariantId { get; set; }

	public string ExternalID { get; set; }

	public long PrintfulStoreProductId { get; set; }

	public long PrintfulVariantId { get; set; }

	public string AvailabilityStatus { get; set; }

	public string Color { get; set; }

	public string Size { get; set; }

	public string Currency { get; set; }

	public string SKU { get; set; }

	public string VariantName { get; set; }

	public decimal RetailPrice { get; set; }

	public long? CategoryId { get; set; }

	public int IsIgnored { get; set; }

	public int IsSynced { get; set; }
	public string? fileUrl { get;set;}
	public string? CategoryName { get; set; }
	public virtual List<VariantFileDTO>? Files { get; set; }

	public virtual StoreProductDTO StoreProduct { get; set;}

	public virtual ProductCategoryDTO ProductCategory { get; set;}

}
