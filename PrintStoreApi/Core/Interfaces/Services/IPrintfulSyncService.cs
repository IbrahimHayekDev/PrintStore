using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.User;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface IPrintfulSyncService
{
	Task<Response<ListResponse<SyncProductsResponse>>> GetProductsToSyncAsync();
	Task<Response<bool>> SyncSingleProductAsync(SyncProductsResponse product);
	Task<Response<ListResponse<ProductCategoryDTO>>> GetPrintfulCategories();
	Task<Response<bool>> SyncCategoriesWithDB();
	Task<Response<ListResponse<ProductCategorySyncDTO>>> GetCategoriesToSync();
	Task<Response<bool>> SyncSingleCategory(ProductCategorySyncDTO categoryToSync);
	Task<Response<string>> SyncCustomizableProducts();
	Task<Response<ProductSizeGuideDTO>> ProductSizeGuideByPIdAsync(long productId);


}
