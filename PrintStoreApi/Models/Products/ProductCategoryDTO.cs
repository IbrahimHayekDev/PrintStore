using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Models.Common;

namespace PrintStoreApi.Models.Products;

public class ProductCategoryDTO
{
	public int Id { get; set; }
	public long PrintfulId { get; set; }
	public int ParentId { get; set; }
	public long ParentPrintfulId { get; set; }
	public string ImageUrl { get; set; }
	public string Size { get; set; }
	public string Title { get; set; }
	public List<ProductCategoryDTO>? SubCategories { get; set; }
}

public class ProductCategorySyncDTO : ProductCategoryDTO
{
	public SyncProductStatus SyncStatus { get; set; }
	public List<difference> Changes { get; set; } = new List<difference>();
	public int dbID { get; set; }
	public string parantTitle { get;set;}
}