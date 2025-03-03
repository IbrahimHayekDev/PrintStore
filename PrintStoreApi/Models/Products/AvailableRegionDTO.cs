using PrintStoreApi.Core.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrintStoreApi.Models.Products.Customizable;

namespace PrintStoreApi.Models.Products;

public class AvailableRegionDTO
{
	public int Id { get; set; }
	public int VariantId { get; set; }
	public string AvailableStatus { get; set; }
	public string RegionName { get; set; }
	public virtual CustomizableVariantDTO? CustomizableVarint { get; set; }
}
