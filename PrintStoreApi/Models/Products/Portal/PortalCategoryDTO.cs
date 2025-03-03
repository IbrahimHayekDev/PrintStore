namespace PrintStoreApi.Models.Products.Portal;

public class PortalCategoryDTO
{
	public int Id { get; set; }
	public long PId {get; set;}
	public string ImageUrl { get; set; }
	public string Title { get; set; }
	public List<PortalCategoryDTO>? SubCategories { get; set; }

}
