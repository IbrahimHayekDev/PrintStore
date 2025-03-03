using PrintStoreApi.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("StoreProducts")]
public class StoreProduct : Entity
{
	//[Key]
	//public int Id { get; set; }

	[Required]
	public long PrintfulId { get; set; }

	[Required]
	public int BaseProductId { get; set; }

	[StringLength(100)]
	public string ExternalID { get; set; }

	[Required, StringLength(255)]
	public string ProductName { get; set; }

	public int VariantCount { get; set; }

	public int SyncedVariantCount { get; set; }

	[StringLength(500)]
	public string ThumbnailUrl { get; set; }

	public int IsIgnored { get; set; }
	public long? CategoryId { get; set; }

	// Navigation properties
	[ForeignKey("BaseProductId")]
	public BaseProduct? BaseProduct { get; set; }
	public ICollection<StoreVariant>? StoreVariants { get; set; }
}
