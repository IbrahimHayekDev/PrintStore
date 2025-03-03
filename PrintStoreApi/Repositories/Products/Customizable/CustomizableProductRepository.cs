using Microsoft.EntityFrameworkCore;

using PrintfulIntegration.Models.v2;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.Products.Portal;
using PrintStoreApi.Reprositories.Base;

using System.Drawing;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrintStoreApi.Repositories.Products.Customizable;

public class CustomizableProductRepository : Reprository<CustomizableProduct>, ICustomizableProductRepository
{
	public CustomizableProductRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<CustomizableProduct> GetProductByPrintfulID(long printfulId)
	{
		return await _context.CustomizableProducts.FirstOrDefaultAsync(product => product.PrintfulId != null && product.PrintfulId == printfulId);
	}
	public async Task<List<CustomizableProductDTO>> GetCustomieProductsByCategortyId(long categoryId)

	{
		var mainCategoryProducts =  await _context.CustomizableProducts
			.Where(p => p.CategoryId == categoryId && p.CustomizableVarints.Any())
			.Select(p => new CustomizableProductDTO
			{
				Id = p.Id,
				PrintfulId = p.PrintfulId,
				CategoryId = p.CategoryId,
				ProductType = p.ProductType,
				ProductTypeName = p.ProductTypeName,
				ProductDescription = p.ProductDescription,
				Title = p.Title,
				Brand = p.Brand,
				ImageUrl = p.ImageUrl,
				Model = p.Model,
				VariantCount = p.VariantCount,
				Currency = p.Currency,
				IsDiscontinued = p.IsDiscontinued,
				AverageFulfilmentTime = p.AverageFulfilmentTime,
				OriginCountry = p.OriginCountry,
				MinPrice = p.CustomizableVarints.Any() ? p.CustomizableVarints.Min(v => v.Price) : 0,
				MaxPrice = p.CustomizableVarints.Any() ? p.CustomizableVarints.Max(v => v.Price) : 0,
			}).ToListAsync();
		return mainCategoryProducts;
	}

	public async Task<CustomizableProductDTO> GetCustomizeProducDetailstbyId(int id)
	{
		var product =  await _context.CustomizableProducts
			.Include(p => p.CustomizableVarints)
			.ThenInclude(v => v.AvailableRegions)
			.FirstOrDefaultAsync(p => p.Id == id);
		if(product == null)
		{
			return null;
		}
		return new CustomizableProductDTO
		{
			AverageFulfilmentTime= product.AverageFulfilmentTime,
			Brand = product.Brand,
			CategoryId = product.CategoryId,
			Currency = product.Currency,
			Id = product.Id,
			ImageUrl = product.ImageUrl,
			IsDiscontinued = product.IsDiscontinued,
			PrintfulId = product.PrintfulId,
			ProductType = product.ProductType,
			ProductTypeName = product.ProductTypeName,
			Title = product.Title,
			ProductDescription = product.ProductDescription,
			Model = product.Model,
			VariantCount = product.VariantCount,
			OriginCountry = product.OriginCountry,
			CustomizableVarints = product.CustomizableVarints!= null ? product.CustomizableVarints.Select(v => new CustomizableVariantDTO
			{
				Id= v.Id,
				PrintfulId = v.PrintfulId,
				BaseProductId = v.BaseProductId,
				PrintfulProductId = v.PrintfulProductId,
				VariantName = v.VariantName,
				Color = v.Color,
				ColorCode1 = v.ColorCode1,
				ColorCode2 = v.ColorCode2,
				ImageUrl = v.ImageUrl,
				Price = v.Price,
				Size = v.Size,
				InStock= v.InStock,
				ProductMaterials= v.ProductMaterials!= null ? v.ProductMaterials.Select(m => new ProductMaterialDTO
				{
					Id = m.Id,
					VariantId = m.VariantId,
					MaterialName = m.MaterialName,
					MaterialPercentage = m.MaterialPercentage,
				}).ToList() : null,
				AvailableRegions = v.AvailableRegions!=null ? v.AvailableRegions.Select(r => new AvailableRegionDTO
				{
					Id = r.Id,
					VariantId = r.VariantId,
					AvailableStatus = r.AvailableStatus,
					RegionName = r.RegionName,
				}).ToList(): null,
			}).ToList() : null
		};
	}

	public async Task<PortaleCategoryDetailsResponseDTO> GetCustomizeProductsByCategortyId(long categoryId)

	{
		var category = await _context.ProductCategories
			.Include(c => c.CustomizableProduct)
			.ThenInclude(p => p.CustomizableVarints)
			.Where(c => c.PrintfulId == categoryId).FirstOrDefaultAsync();
		if(category == null)
			return null;

		var categoryDto = new PortaleCategoryDetailsResponseDTO
		{
			Id = category.Id,
			Name = category.Title,
			Products = category.CustomizableProduct.Where(p => p.CustomizableVarints.Any() && p.IsDiscontinued == 0).Select(p => new SingleProductListDTO
			{
				Id = p.Id,
				Title = p.Title,
				Brand = p.Brand,
				ImageUrl = p.ImageUrl,
				Currency = p.Currency,
				MinPrice = p.CustomizableVarints.Any() ? p.CustomizableVarints.Min(v => v.Price) : 0,
				MaxPrice = p.CustomizableVarints.Any() ? p.CustomizableVarints.Max(v => v.Price) : 0,
			}).ToList()
		};

		categoryDto.SubCategories = await GetSubCategories(category.Id);

		return categoryDto;
	}

	
	private async Task<List<PortalMainCategoryDTO>> GetSubCategories(int parentId)
	{

		var categories =   await _context.ProductCategories
		   .Where(c => c.ParentId == parentId && c.CustomizableProduct.Any())
		   .ToListAsync();
		return categories.Select(MapToPortalMainCategoryDTO).ToList();

	}

	private PortalMainCategoryDTO MapToPortalMainCategoryDTO(ProductCategory category)
	{
		return new PortalMainCategoryDTO
		{
			Id = category.Id,
			Title = category.Title,
			PId = category.PrintfulId,
		};
	}
}