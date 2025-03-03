using PrintStoreApi.Models.Products.Customizable;

namespace PrintStoreApi.Models.Products.Portal;

public class PortaleCategoryDetailsResponseDTO
{
	public int Id { get; set; }
	public string Name { get; set; }
	public List<SingleProductListDTO>? Products { get; set; }
	public List<PortalMainCategoryDTO>? SubCategories { get; set; }

}