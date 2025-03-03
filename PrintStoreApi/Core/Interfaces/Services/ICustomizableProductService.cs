using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Products.Customizable;
using PrintStoreApi.Models.Products.Portal;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface ICustomizableProductService
{
	 Task<Response<PortaleCategoryDetailsResponseDTO>> GetCustomizeProductsByCategortyId(long categoryId);

	 Task<Response<CustomizableProductDTO>> GetCustomizeProducDetailstbyId(int id);

}
