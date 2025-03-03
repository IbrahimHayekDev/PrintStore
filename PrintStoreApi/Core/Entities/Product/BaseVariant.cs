using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrintStoreApi.Core.Entities.Base;

namespace PrintStoreApi.Core.Entities.Product;

[Table("BaseVariants")]
public class BaseVariant : Entity
{
	//[Key]
	//public int Id { get; set; }

	[Required]
	public long PrintfulId { get; set; }

	[Required]
	public int BaseProductId { get; set; }

	[Required]
	public long PrintfulProductId { get; set; }

	[StringLength(100)]
	public string VariantName { get; set; }

	[StringLength(50)]
	public string Color { get; set; }

	[StringLength(50)]
	public string? ColorCode1 { get; set; }

	[StringLength(50)]
	public string? ColorCode2 { get; set; }

	[StringLength(500)]
	public string ImageUrl { get; set; }

	[Column(TypeName = "decimal(10, 2)")]
	public decimal Price { get; set; }

	[StringLength(50)]
	public string Size { get; set; }

	public int InStock { get; set; }

	// Navigation properties
	public BaseProduct? BaseProduct { get; set; }
	public ICollection<StoreVariant>? StoreVariants { get; set; }

	public static implicit operator Task<object>(BaseVariant v)
	{
		throw new NotImplementedException();
	}
}
