using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface IProductCategoryService
{
	 Task<Response<ListResponse<ProductCategoryDTO>>> GetAllCategoriesAsync();
	 Task<Response<ListResponse<PortalCategoryDTO>>> GetCategoriesWithProducts();
	 Task<Response<ListResponse<PortalCategoryDTO>>> GetCategoriesWithCustomizableProducts();

	//Portal
	  Task<Response<ListResponse<PortalCategoryDTO>>> GetPortalCategoriesById(int categoryId);
	  Task<Response<ListResponse<PortalMainCategoryDTO>>> GetPortalMainCategories();


}
