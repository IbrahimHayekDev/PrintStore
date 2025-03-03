using PrintStoreApi.Core.Entities.Product;

namespace PrintStoreApi.Models.Products;

public class VariantFileDTO
{
	public long PrintfultId { get; set; }
	public int Id { get; set; }
	public string NameFile { get; set; }
	public string MimeType { get; set; }
	public int? CreatedAt { get; set; }
	public string ThumbnailUrl { get; set; }
	public string PreviewUrl { get; set; }
	public int VariantId { get; set; }


}
