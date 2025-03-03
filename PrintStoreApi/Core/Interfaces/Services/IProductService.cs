

using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface IProductService
{
 Task<Response<ListResponse<StoreProductDTO>>> GetStoreProducts();
 Task<Response<StoreProductDTO>> GetStoreProductById(string id);
Task<Response<ListResponse<GetStoreVariantByProductIdResponse>>> GetStoreVariantByProductId(int id);
Task<Response<ListResponse<PortalStoreProductDTO>>> GetStoreProductsByCategortyId(long categoryId);


}
