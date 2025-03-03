using PrintfulIntegration.Models.common;
using PrintfulIntegration.Models.Products;

namespace PrintfulIntegration.Core.Interfaces.Services;

public interface IPrintfulCategotyService
{
	Task<Response<PrintfulCategoryResponse>> GetPrintfulCategories();
}
