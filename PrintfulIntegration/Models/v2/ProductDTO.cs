using PrintfulIntegration.Models.Products.ProductCatalog;

namespace PrintfulIntegration.Models.v2;

public class ProductDTO
{
	public int id { get; set; }
	public int main_category_id { get; set; }
	public string type { get; set; }
	public string name { get; set; }
	public string brand { get; set; }
	public string model { get; set; }
	public string image { get; set; }
	public int variant_count { get; set; }
	public bool is_discontinued { get; set; }
	public string description { get; set; }
	public List<string> sizes { get; set; }
	public List<Color> colors { get; set; }
	public List<Technique> techniques { get; set; }
	public List<Placement> placements { get; set; }
	public List<ProductOption> product_options { get; set; }
	public Links _links { get; set; }
}

public class Availability
{
	public string href { get; set; }
}

public class Categories
{
	public string href { get; set; }
}

public class Color
{
	public string name { get; set; }
	public string value { get; set; }
}
public class Layer
{
	public string type { get; set; }
	public List<LayerOption> layer_options { get; set; }
}

public class LayerOption
{
	public string name { get; set; }
	public List<string> techniques { get; set; }
	public string type { get; set; }
	public List<bool> values { get; set; }
}

public class Links
{
	public Self self { get; set; }
	public Variants variants { get; set; }
	public Categories categories { get; set; }
	public ProductPrices product_prices { get; set; }
	public ProductSizes product_sizes { get; set; }
	public ProductImages product_images { get; set; }
	public Availability availability { get; set; }
}

public class Placement
{
	public string placement { get; set; }
	public string technique { get; set; }
	public int print_area_width { get; set; }
	public int print_area_height { get; set; }
	public List<Layer> layers { get; set; }
	public List<PlacementOption> placement_options { get; set; }
	public List<string> conflicting_placements { get; set; }
}

public class PlacementOption
{
	public string name { get; set; }
	public List<string> techniques { get; set; }
	public string type { get; set; }
	public List<bool> values { get; set; }
}

public class ProductImages
{
	public string href { get; set; }
}

public class ProductOption
{
	public string name { get; set; }
	public List<string> techniques { get; set; }
	public string type { get; set; }
	public List<string> values { get; set; }
}

public class ProductPrices
{
	public string href { get; set; }
}

public class ProductSizes
{
	public string href { get; set; }
}


public class Self
{
	public string href { get; set; }
}

public class Technique
{
	public string key { get; set; }
	public string display_name { get; set; }
	public bool is_default { get; set; }
}

public class Variants
{
	public string href { get; set; }
}
