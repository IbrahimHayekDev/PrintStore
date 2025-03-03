using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface IMaterialRepository : IReprository<ProductMaterial>
{
	 Task<ProductMaterial> GetMaterialByPrintfulID(long printfulId, string materialName);

}
