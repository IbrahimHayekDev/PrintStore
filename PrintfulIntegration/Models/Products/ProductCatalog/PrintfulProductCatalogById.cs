namespace PrintfulIntegration.Models.Products.ProductCatalog;

public class PrintfulProductCatalogById
{
	public PrintfulCatalogProduct product { get; set; }
	public List<PrintfulCatalogVariant> variants { get; set; }
}
