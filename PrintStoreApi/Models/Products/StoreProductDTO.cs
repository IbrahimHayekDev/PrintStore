using PrintStoreApi.Core.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrintStoreApi.Core.Entities.Base;

namespace PrintStoreApi.Models.Products;

public class StoreProductDTO
{
	public int Id { get; set;}
	public long PrintfulId { get; set; }

	public int BaseProductId { get; set; }

	public string ExternalID { get; set; }

	public string ProductName { get; set; }

	public int VariantCount { get; set; }

	public int SyncedVariantCount { get; set; }

	public string ThumbnailUrl { get; set; }

	public int IsIgnored { get; set; }
	public ICollection<StoreVariantDTO>? StoreVariants { get; set; } = new List<StoreVariantDTO>();


}
