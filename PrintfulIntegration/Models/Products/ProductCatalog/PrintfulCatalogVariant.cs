namespace PrintfulIntegration.Models.Products.ProductCatalog;

public class PrintfulCatalogVariant
{
	public long id { get; set; }
	public int product_id { get; set; }
	public string name { get; set; }
	public string size { get; set; }
	public string color { get; set; }
	public string? color_code { get; set; }
	public string? color_code2 { get; set; }
	public string image { get; set; }
	public string price { get; set; }
	public bool in_stock { get; set; }
	public  Dictionary<string,string>? availability_regions { get; set; }
	public List<AvailabilityStatus>? availability_status { get; set; }
	public List<Material>? material { get; set; }
}

public class AvailabilityStatus
{
	public string region { get; set; }
	public string status { get; set; }
}
public class Material
{
	public string name { get; set; }
	public decimal percentage { get; set; }
}