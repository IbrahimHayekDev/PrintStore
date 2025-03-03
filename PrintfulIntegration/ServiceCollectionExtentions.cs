using PrintfulIntegration.Core.Interfaces.Services;
using PrintfulIntegration.Mapping;
using PrintfulIntegration.Services;

namespace PrintfulIntegration;

public static class ServiceCollectionExtentions
{
	public static IServiceCollection AddPrintfulServices(this IServiceCollection services)
	{
		// Register AutoMapper frpm PrintfulIntegration
		services.AddAutoMapper(typeof(PrintfulProductMappingProfile));
		services.AddScoped<IPrintfulProductService,PrintfulProductService>();
		services.AddScoped<IPrintfulCategotyService, PrintfulCategotyService>();
		services.AddHttpClient<PrintfulProductService>();
		return services;
	}
}
