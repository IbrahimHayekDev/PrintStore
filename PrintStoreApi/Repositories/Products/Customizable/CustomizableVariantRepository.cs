using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products.Customizable;

public class CustomizableVariantRepository: Reprository<CustomizableVarint>, ICustomizableVariantRepository
{
	public CustomizableVariantRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<CustomizableVarint> GetVariantByPrintfulID(long printfulId)
	{
		return await _context.CustomizableVarints.FirstOrDefaultAsync(variant => variant.PrintfulId != null && variant.PrintfulId == printfulId);
	}

}
