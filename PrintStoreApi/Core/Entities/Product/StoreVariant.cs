using PrintStoreApi.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("StoreVariants")]
public class StoreVariant : Entity
{
	//[Key]
	//public int Id { get; set; }

	[Required]
	public long PrintfulId { get; set; }

	[Required]
	public int BaseProductId { get; set; }

	[Required]
	public int StoreProductId { get; set; }

	[Required]
	public int BaseVariantId { get; set; }

	[StringLength(100)]
	public string ExternalID { get; set; }

	[Required]
	public long PrintfulStoreProductId { get; set; }

	[Required]
	public long PrintfulVariantId { get; set; }

	[StringLength(50)]
	public string AvailabilityStatus { get; set; }

	[StringLength(50)]
	public string Color { get; set; }

	[StringLength(50)]
	public string Size { get; set; }

	[StringLength(50)]
	public string Currency { get; set; }

	[StringLength(100)]
	public string SKU { get; set; }

	[StringLength(255)]
	public string VariantName { get; set; }

	[Column(TypeName = "decimal(10, 2)")]
	public decimal RetailPrice { get; set; }

	public long? CategoryId { get; set; }

	public int IsIgnored { get; set; }

	public int IsSynced { get; set; }

	// Navigation properties
	public List<VariantFile>? Files { get; set; }
	public BaseProduct? BaseProduct { get; set; }
	public StoreProduct? StoreProduct { get; set; }
	public BaseVariant? BaseVariant { get; set; }
	public virtual ProductCategory? ProductCategory { get; set; }

}
