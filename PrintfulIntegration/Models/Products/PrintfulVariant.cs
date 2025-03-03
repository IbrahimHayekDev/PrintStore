namespace PrintfulIntegration.Models.Products;

public class PrintfulVariant
{
	public long id { get; set; }
	public string external_id { get; set; }
	public int sync_product_id { get; set; }
	public string name { get; set; }
	public bool synced { get; set; }
	public int variant_id {  get; set; }
	public string retail_price { get; set; }
	public string currency { get; set; }
	public bool is_ignored {  get; set; }
	public string? sku {  get; set; }
	public PrintfulVariantProduct product { get; set; }
	public List<VariantFiles> files { get; set; }
	public List<VariantOptions> options { get; set; }
	public int? main_category_id { get; set; }
	public int? warehouse_product_id { get; set; }
	public int? warehouse_product_variant_id { get; set; }
	public string? size { get; set; }
	public string? color { get; set; }
	public string availability_status { get; set; }
}


public class VariantOptions
{
	public string id { get; set; }
	public object value { get; set; }
}