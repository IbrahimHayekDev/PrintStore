using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;
public class BaseProductRepository : Reprository<BaseProduct>, IBaseProductRepository
{
	public BaseProductRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<BaseProduct> GetBaseProductByPrintfulID(string printfulId)
	{
		return await _context.BaseProducts.FirstOrDefaultAsync(product => product.PrintfulId != null && product.PrintfulId.ToString() == printfulId.ToString());
	}
}
