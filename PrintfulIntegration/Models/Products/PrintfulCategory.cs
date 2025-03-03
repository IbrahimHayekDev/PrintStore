namespace PrintfulIntegration.Models.Products;

public class PrintfulCategory
{
	public long id {  get; set; }
	public long parent_id {  get; set; }
	public string image_url { get; set; }
	public string size { get; set; }
	public string title { get; set; }

}

public class PrintfulCategoryResponse
{
	public List<PrintfulCategory> categories { get; set;} = [];
}