using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface IStoreVariantRepository : IReprository<StoreVariant>
{
	Task<StoreVariant> GetStoreVariantByPrintfulID(string printfulId);
	Task<List<StoreVariant>> GetStoreVariantsByProductID(string productId);
	 Task<StoreVariant> GetStoreVariantWithFiles(int id);
	 Task<List<StoreVariant>> GetVariantsByProductId(int id);


}