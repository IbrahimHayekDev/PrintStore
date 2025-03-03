using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Reprositories.Base;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Core.Interfaces.Repositories.Products;

public interface ICustomizableProductRepository: IReprository<CustomizableProduct>
{
 Task<PortaleCategoryDetailsResponseDTO> GetCustomizeProductsByCategortyId(long categoryId);
 Task<CustomizableProduct> GetProductByPrintfulID(long printfulId);
 Task<CustomizableProductDTO> GetCustomizeProducDetailstbyId(int id);


}
