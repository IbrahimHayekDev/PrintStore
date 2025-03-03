using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class BaseVariantRepository : Reprository<BaseVariant>, IBaseVariantRepository
{
	public BaseVariantRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<BaseVariant> GetBaseVariantByPrintfulID(string printfulId)
	{
		return await _context.BaseVariants.FirstOrDefaultAsync(variant => variant.PrintfulId.ToString() == printfulId.ToString());
	}

}
