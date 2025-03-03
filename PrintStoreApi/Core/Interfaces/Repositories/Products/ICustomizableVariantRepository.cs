using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface ICustomizableVariantRepository : IReprository<CustomizableVarint>
{
	Task<CustomizableVarint> GetVariantByPrintfulID(long printfulId);

}
