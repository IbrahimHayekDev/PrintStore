namespace PrintfulIntegration.Models.Products;

public class PrintfulProduct
{
	public long id { get; set; }
	public string external_id { get; set; }
	public string name { get; set; }
	public string thumbnail_url { get; set; }
	public int variants { get; set; }
	public int synced { get; set; }
	public bool is_ignored { get; set; }
}
