using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Portal;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class ProductCategoryRepository : Reprository<ProductCategory>, IProductCategoryRepository
{
	public ProductCategoryRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<ProductCategory> GetCategoryByPrintfulID(long printfulId)
	{
		return await _context.ProductCategories.FirstOrDefaultAsync(category => category.PrintfulId == printfulId);
	}

	public async Task<List<ProductCategoryDTO>> GetNestedCategoriesAsync()
	{
		var categories = await _context.ProductCategories
			.Include(c => c.SubCategories)
			.Where(c => c.ParentId ==  null)
			.ToListAsync();
		return categories.Select(MapToCategoryDTO).ToList();
	}

	private ProductCategoryDTO MapToCategoryDTO(ProductCategory category)
	{
		return new ProductCategoryDTO
		{
			Id = category.Id,
			Title = category.Title,
			ImageUrl = category.ImageUrl,
			PrintfulId = category.PrintfulId,
			SubCategories = category.SubCategories?.Select(MapToCategoryDTO).ToList(),
		};
	}

	public async Task<List<ProductCategory>> GetCategoriesWithProducts()
	{
		var result =  _context.ProductCategories.Where(c => c.BaseProducts.Any()).ToList();
		return result;
	}

	public async Task<List<ProductCategory>> GetCategoriesWithCustomizableProducts()
	{
		var result = await _context.ProductCategories.Where(c => c.CustomizableProduct.Any()).ToListAsync();
		return result;
	}

	// Portal

	public async Task<List<PortalCategoryDTO>> GetPortalCategoriesById(int categoryId)
	{
		var categories = await _context.ProductCategories
			.Where(c => c.ParentId == categoryId
			&&
			(_context.CustomizableProducts.Any(p => p.CategoryId == c.PrintfulId && p.IsDiscontinued == 0) ||
			_context.ProductCategories.Any(sub => sub.ParentId == c.Id && 
			_context.CustomizableProducts.Any(p => p.CategoryId == sub.PrintfulId && p.IsDiscontinued == 0))))
			.Distinct()
			.ToListAsync();

		//var categories = await _context.ProductCategories
		//	.Include(c => c.SubCategories)
		//	.Where(c => c.ParentId == categoryId && c.CustomizableProduct.Any())
		//	.ToListAsync();
		return categories.Select(MapToPortalCategoryDTO).ToList();
	}

	private PortalCategoryDTO MapToPortalCategoryDTO(ProductCategory category)
	{
		return new PortalCategoryDTO
		{
			Id = category.Id,
			Title = category.Title,
			ImageUrl = category.ImageUrl,
			PId = category.PrintfulId,
			SubCategories = category.SubCategories?.Select(MapToPortalCategoryDTO).ToList(),
		};
	}

	public async Task<List<PortalMainCategoryDTO>> GetPortalMainCategories()
	{

		var categories = await _context.ProductCategories
			.Where(main => main.ParentId == null
			&& 
			_context.ProductCategories.Any( sub => sub.ParentId == main.Id && 
			_context.CustomizableProducts.Any(p => p.CategoryId == sub.PrintfulId && p.IsDiscontinued == 0)))
			.Distinct()
			.ToListAsync();

		return categories.Select(MapToPortalMainCategoryDTO).ToList();
		//var categories = await _context.ProductCategories
		//	.Where(c => c.ParentId == null )
		//	.Where(c => _context.ProductCategories
		//		.Where(subCategory => subCategory.ParentId == c.Id)
		//		.Join(_context.CustomizableProducts, subcategory => subcategory.PrintfulId, product => product.CategoryId, (subCat, product) => subCat).Any()).ToListAsync();

		//return categories.Select(MapToPortalMainCategoryDTO).ToList();
	}

	private PortalMainCategoryDTO MapToPortalMainCategoryDTO(ProductCategory category)
	{
		return new PortalMainCategoryDTO
		{
			Id = category.Id,
			Title = category.Title,
		};
	}
}