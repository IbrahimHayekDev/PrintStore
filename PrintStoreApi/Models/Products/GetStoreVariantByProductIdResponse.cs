namespace PrintStoreApi.Models.Products;

public class GetStoreVariantByProductIdResponse
{
	public int Id { get; set; }

	public long PrintfulId { get; set; }

	public string AvailabilityStatus { get; set; }

	public string Color { get; set; }

	public string Size { get; set; }

	public string Currency { get; set; }

	public string SKU { get; set; }

	public string VariantName { get; set; }

	public decimal RetailPrice { get; set; }

	public long? CategoryId { get; set; }
	public string? CategoryName { get; set; }

	public int IsIgnored { get; set; }

	public int IsSynced { get; set; }
	public string? fileUrl { get; set; }

}
