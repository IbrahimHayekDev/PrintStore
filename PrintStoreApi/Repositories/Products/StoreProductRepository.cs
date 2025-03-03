using Microsoft.EntityFrameworkCore;
using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Models.Products.Portal;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class StoreProductRepository : Reprository<StoreProduct>, IStoreProductRepository
{
	public StoreProductRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<StoreProduct> GetStoreProductByPrintfulID(string printfulId)
	{
		return await _context.StoreProducts.FirstOrDefaultAsync(product => product.PrintfulId.ToString() == printfulId.ToString());
	}

	public async Task<List<StoreProduct>> GetAllProductsWithVariants()
	{
		return await _context.StoreProducts.Include(p => p.StoreVariants).ToListAsync();
	}

	public async Task<List<PortalStoreProductDTO>> GetStoreProductsByCategortyId(long categoryId)
	{
		return await _context.StoreProducts
			.Where(p => p.CategoryId == categoryId)
			.Select(p => new PortalStoreProductDTO {
				Id = p.Id,
				PrintfulId = p.PrintfulId,
				BaseProductId = p.BaseProductId,
				ProductName = p.ProductName,
				VariantCount = p.VariantCount,
				ThumbnailUrl = p.ThumbnailUrl,
				IsIgnored = p.IsIgnored,
				MinPrice = p.StoreVariants.Any() ? p.StoreVariants.Min(v => v.RetailPrice) : 0,
				MaxPrice = p.StoreVariants.Any() ? p.StoreVariants.Max(v => v.RetailPrice) : 0,
				Currency = p.StoreVariants.Any() ? p.StoreVariants.First().Currency : null
			}).ToListAsync();
	}
}
