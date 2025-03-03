namespace PrintfulIntegration.Models.Products.ProductCatalog;

public class PrintfulCatalogProduct
{
	public int id { get; set; }
	public int main_category_id { get; set; }
	public string type { get; set; }
	public string type_name { get; set; }
	public string title { get; set; }
	public string brand { get; set; }
	public string model { get; set; }
	public string image { get; set; }
	public int variant_count { get; set; }
	public string currency { get; set; }
	public List<PrintfulCatalogProductFile> files { get; set; }
	public string ProductName { get; set; }
	public List<optionValue>? options { get; set; }
	public bool is_discontinued { get; set; }
	public decimal? avg_fulfillment_time { get; set; }
	public string description { get; set; }
	public List<technique>? techniques { get; set; }
	public string? origin_country { get; set; }
}

public class PrintfulCatalogProductFile
{
	public string id { get; set;}
	public string type { get; set;}
	public string title { get; set;}
	public string? additional_price { get; set;}
	public string ProductName { get; set;}
	public List<PrintfulCatalogProductFileOption>? options { get; set;}
}

public class PrintfulCatalogProductFileOption
{
	public string id { get; set; }
	public string type { get; set;}
	public string title { get; set;}
	public double additional_price { get; set; }
}

public class PrintfulCatalogProductOption
{
	public string id { get; set;}
	public string title { get; set;}
	public string type { get; set;}
	public string additional_price { get; set; }
}

public class optionValue
{
	public string id { get; set;}
	public string title { get; set;}
	public string type { get; set;}
	public dynamic values { get; set; }
	public string additional_price { get; set;}
	public dynamic? additional_price_breakdown { get; set; }

}

public class technique
{
	public string key { get; set; }
	public string display_name { get; set; }
	public bool is_default { get; set; }

}
