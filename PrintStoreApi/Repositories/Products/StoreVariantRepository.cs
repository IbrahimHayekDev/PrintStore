using Microsoft.EntityFrameworkCore;
using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Entities.User;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class StoreVariantRepository : Reprository<StoreVariant>, IStoreVariantRepository
{
	public StoreVariantRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<StoreVariant> GetStoreVariantByPrintfulID(string printfulId)
	{
		return await _context.StoreVariants.FirstOrDefaultAsync(variant => variant.PrintfulId.ToString() == printfulId.ToString());
	}

	public async Task<List<StoreVariant>> GetStoreVariantsByProductID(string productId)
	{
		return await _context.StoreVariants.Where(variant => variant.StoreProductId.ToString() == productId.ToString()).ToListAsync();
	}

	public async Task<StoreVariant> GetStoreVariantWithFiles(int id)
	{
		return await _context.StoreVariants.Include(v => v.Files).FirstOrDefaultAsync(v => v.Id == id);
	}

	public async Task<List<StoreVariant>> GetVariantsByProductId(int id)
	{
		return await _context.StoreVariants.Include(v => v.Files).Include(v => v.ProductCategory).Where(v => v.StoreProductId == id).ToListAsync();
	}

}
