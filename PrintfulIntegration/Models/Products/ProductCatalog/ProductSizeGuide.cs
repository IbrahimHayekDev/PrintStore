namespace PrintfulIntegration.Models.Products.ProductCatalog;

public class ProductSizeGuide
{
	public long product_id { get; set; }
	public List<dynamic> available_sizes { get; set; }
	public List<SizeTable> size_tables { get; set; }
}

public class SizeTable
{
	public string type { get; set; }
	public string? unit { get; set; }
	public string? description { get; set; }
	public string? image_url { get; set; }
	public string? image_description { get; set; }
	public List<Measurement> measurements { get; set; }
}

public class Measurement
{
	public string? type_label { get; set; }
	public string? unit { get; set; }
	public List<MeasurementValue> values { get; set; }
}


public class MeasurementValue
{
	public string size { get; set; }
	public string? value { get; set; }
	public string? min_value { get; set; }
	public string? max_value { get; set; }
}
