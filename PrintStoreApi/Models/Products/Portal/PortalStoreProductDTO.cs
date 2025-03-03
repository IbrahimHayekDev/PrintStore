namespace PrintStoreApi.Models.Products.Portal;

public class PortalStoreProductDTO
{
	public int Id { get; set; }
	public long PrintfulId { get; set; }

	public int BaseProductId { get; set; }

	public string ProductName { get; set; }

	public int VariantCount { get; set; }

	public string ThumbnailUrl { get; set; }

	public int IsIgnored { get; set; }

	public decimal MinPrice {  get; set; }

	public decimal MaxPrice { get; set; }
	public string? Currency { get; set; }
}
