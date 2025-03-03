using PrintStoreApi.Core.Entities.Product;

namespace PrintStoreApi.Models.Products.Customizable;

public class SingleProductListDTO
{
	public string? Brand { get; set; }
	public string Currency { get; set; }

	public int Id { get; set; }

	public string ImageUrl { get; set; }

	public decimal MinPrice { get; set; }

	public decimal MaxPrice { get; set; }

	public string Title { get; set; }

}
