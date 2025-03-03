namespace PrintfulIntegration.Models.Products;

public class PrintfulProductById
{
	public PrintfulProduct sync_product {  get; set; }
	public List<PrintfulVariant> sync_variants { get; set; }
}
