using PrintfulIntegration.Models.Products.ProductCatalog;

namespace PrintfulIntegration.Models.Products;

public class VariantFiles
{
	public string type { get; set; }
	public int id { get; set; }
	public string url { get; set; }
	public List<VariantFileOptions> options { get; set; }
	public string hash { get; set; }
	public string filename { get; set; }
	public string mime_type { get; set; }
	public int size { get; set; }
	public int width { get; set; }
	public int height { get; set; }
	public int? dpi { get; set; }
	public string status { get; set; }
	public int created { get; set; }
	public string thumbnail_url { get; set; }
	public string preview_url { get; set; }
	public bool visible {  get; set; }
	public bool is_temporary {  get; set; }
	public string? stitch_count_tier {  get; set; }





}

public class VariantFileOptions
{
	public string id { get; set; }
	public Object? value { get; set; }
}

public class OptionValue
{
	public string validationHash { get; set; }
	public bool isValid { get; set; }
}