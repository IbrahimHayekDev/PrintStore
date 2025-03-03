using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface IProductCategoryRepository : IReprository<ProductCategory>
{
	Task<ProductCategory> GetCategoryByPrintfulID(long printfulId);
	 Task<List<ProductCategoryDTO>> GetNestedCategoriesAsync();
	 Task<List<ProductCategory>> GetCategoriesWithProducts();
	Task<List<ProductCategory>> GetCategoriesWithCustomizableProducts();

	// Portal
	 Task<List<PortalCategoryDTO>> GetPortalCategoriesById(int categoryId);
	 Task<List<PortalMainCategoryDTO>> GetPortalMainCategories();


}
