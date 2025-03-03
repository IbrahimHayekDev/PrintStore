using PrintStoreApi.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("Materials")]
public class ProductMaterial : Entity
{
	[Required]
	public int VariantId { get; set;}
	[StringLength(100)]
	public string MaterialName { get;set; }
	[Column(TypeName = "decimal(10, 2)")]
	public decimal MaterialPercentage { get;set;}
	public virtual CustomizableVarint? CustomizableVarint { get; set; }

}
