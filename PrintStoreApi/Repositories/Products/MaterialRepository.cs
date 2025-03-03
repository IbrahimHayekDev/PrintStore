using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class MaterialRepository : Reprository<ProductMaterial>, IMaterialRepository
{
	public MaterialRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<ProductMaterial> GetMaterialByPrintfulID(long printfulId, string materialName)

	{
		return await _context.Materials.FirstOrDefaultAsync(material => material.VariantId != null && material.VariantId == printfulId && material.MaterialName == materialName);
	}
}