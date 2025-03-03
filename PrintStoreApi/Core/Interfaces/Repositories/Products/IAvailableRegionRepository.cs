using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface IAvailableRegionRepository : IReprository<AvailableRegion>
{
	Task<AvailableRegion> GetRegionByProductID(long printfulId, string regionName);
}
