using PrintStoreApi.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("ProductCategories")]
public class ProductCategory : Entity
{
	[Required]
	public long PrintfulId { get; set; }

	public int? ParentId { get; set; }

	public long ParentPrintfulId { get; set; }

	[StringLength(200)]
	public string ImageUrl { get; set; }

	[StringLength(100)]
	public string Size { get; set; }

	[StringLength(500)]
	public string Title { get; set; }

	public virtual ICollection<ProductCategory>? SubCategories { get;set; } = new List<ProductCategory>();
	public virtual ProductCategory? ParentCategory { get; set; }

	public virtual ICollection<BaseProduct>? BaseProducts { get; set; } = new List<BaseProduct>();
	public virtual ICollection<CustomizableProduct>? CustomizableProduct { get; set; } = new List<CustomizableProduct>();

	public virtual ICollection<StoreVariant>? StoreVariants { get; set; } = new List<StoreVariant>();

}
