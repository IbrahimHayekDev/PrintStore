using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class AvailableRegionRepository : Reprository<AvailableRegion>, IAvailableRegionRepository
{
	public AvailableRegionRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<AvailableRegion> GetRegionByProductID(long printfulId, string regionName)

	{
		return await _context.AvailableRegions.FirstOrDefaultAsync(region => region.VariantId != null && region.VariantId == printfulId && region.RegionName == regionName);
	}
}
