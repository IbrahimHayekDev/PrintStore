using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface IStoreProductRepository : IReprository<StoreProduct>
{
	Task<StoreProduct> GetStoreProductByPrintfulID(string printfulId);
	Task<List<StoreProduct>> GetAllProductsWithVariants();
	Task<List<PortalStoreProductDTO>> GetStoreProductsByCategortyId(long categoryId);


}