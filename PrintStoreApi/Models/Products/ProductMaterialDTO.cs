using PrintStoreApi.Core.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrintStoreApi.Models.Products.Customizable;

namespace PrintStoreApi.Models.Products;

public class ProductMaterialDTO
{
	public int Id { get; set; }
	public int VariantId { get; set; }
	public string MaterialName { get; set; }
	public decimal MaterialPercentage { get; set; }
	public virtual CustomizableVariantDTO? CustomizableVarint { get; set; }
}
