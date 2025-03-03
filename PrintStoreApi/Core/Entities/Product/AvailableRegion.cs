using PrintStoreApi.Core.Entities.Base;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("AvailableRegions")]
public class AvailableRegion : Entity
{
	[Required]
	public int VariantId { get; set;}
	[StringLength(50)]
	public string RegionName { get;set; }
	[StringLength(50)]
	public string AvailableStatus { get; set; }
	public virtual CustomizableVarint? CustomizableVarint { get; set; }

}
