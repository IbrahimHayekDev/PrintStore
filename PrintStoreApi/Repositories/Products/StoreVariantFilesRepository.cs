using Microsoft.EntityFrameworkCore;
using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Data;
using PrintStoreApi.Reprositories.Base;

namespace PrintStoreApi.Repositories.Products;

public class StoreVariantFilesRepository : Reprository<VariantFile>, IStoreVariantFilesRepository
{
	public StoreVariantFilesRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}
}
