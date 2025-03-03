using PrintfulIntegration.Models.common;
using PrintfulIntegration.Models.Products;
using PrintfulIntegration.Models.Products.ProductCatalog;



namespace PrintfulIntegration.Core.Interfaces.Services;

public interface IPrintfulProductService
{
	//Task<Response<ResponseList<ProductDto>>> GetProducts();
	Task<Response<ResponseList<PrintfulProduct>>> GetStoreProductsAsync();
	Task<Response<PrintfulProductById>> GetStoreProductByIdAsync(string Id);
	Task<Response<PrintfulProductCatalogById>> GetProductByIdAsync(long Id);
	Task<Response<ResponseList<PrintfulCatalogProduct>>> GetAllBaseProducts();
	Task<Response<ProductSizeGuide>> GetSizeGuideByProductId(long ProductId);

}
