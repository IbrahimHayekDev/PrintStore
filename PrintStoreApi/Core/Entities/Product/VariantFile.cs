using PrintStoreApi.Core.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.Product;

[Table("StoreVariantFiles")]
public class VariantFile : Entity
{
	public long PrintfultId {  get; set; }
	public string NameFile { get; set; }
	public string MimeType { get; set; }
	public int? CreatedAt { get; set; }
	public string ThumbnailUrl { get; set; }
	public string PreviewUrl { get; set; }
	public int VariantId {  get; set; }

	// Navigation properties
	public virtual StoreVariant? storeVariant { get; set; }
	
}