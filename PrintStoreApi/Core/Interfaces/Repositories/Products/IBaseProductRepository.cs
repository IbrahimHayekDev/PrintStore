using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;


public interface IBaseProductRepository : IReprository<BaseProduct>
{
	Task<BaseProduct> GetBaseProductByPrintfulID(string printfulId);
}