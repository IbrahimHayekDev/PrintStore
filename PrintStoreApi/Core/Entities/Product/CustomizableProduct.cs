using PrintStoreApi.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("CustomizableProducts")]
public class CustomizableProduct : Entity
{
	//[Key]
	//public int Id { get; set; }

	[Required]
	public long PrintfulId { get; set; }

	public long CategoryId { get; set; }

	[StringLength(255)]
	public string ProductType { get; set; }

	[StringLength(255)]
	public string ProductTypeName { get; set; }

	public string ProductDescription { get; set; }

	[Required, StringLength(255)]
	public string Title { get; set; }

	[StringLength(255)]
	public string? Brand { get; set; }

	[StringLength(500)]
	public string ImageUrl { get; set; }

	[StringLength(255)]
	public string Model { get; set; }

	public int VariantCount { get; set; }

	[StringLength(100)]
	public string Currency { get; set; }

	public int IsDiscontinued { get; set; }

	public decimal? AverageFulfilmentTime { get; set; }

	[StringLength(50)]
	public string? OriginCountry { get; set; }

	// Navigation properties
	public ICollection<CustomizableVarint>? CustomizableVarints { get; set; }
	public virtual ProductCategory? ProductCategory { get; set; }

}
