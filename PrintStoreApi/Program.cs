using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using PrintStoreApi.Data;
using PrintStoreApi.Services.Auth;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Repositories;
using PrintStoreApi.Services.User;
using PrintStoreApi.Core.Interfaces.Repositories;
using PrintfulIntegration.Services;
using PrintfulIntegration.configuration;
using PrintfulIntegration.Core.Interfaces.Services;
using PrintStoreApi.Services.Product;
using PrintfulIntegration;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Repositories.Products;
using PrintStoreApi.Configuration;
using PrintStoreApi.Repositories.Products.Customizable;
using PrintStoreApi.Services.Product.Customizable;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Allow CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
	policy =>
	{
		var allowedMethods = builder.Configuration.GetSection("CorsOptions:Methods").Get<string[]>();
		var allowedOrigins = builder.Configuration.GetSection("CorsOptions:Origins").Get<string[]>();
			policy.WithOrigins(allowedOrigins)
			.WithMethods(allowedMethods)
            .AllowAnyHeader();
        });
});
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity configuration

// JWT configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtConfig:Key").Get<string>());
		options.RequireHttpsMetadata = false; // Set to true in production
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Get<string>(),
			ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Get<string>(),
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine($"Authentication failed: {context.Exception.Message}");
				return Task.CompletedTask;
			},
			OnTokenValidated = context =>
			{
				Console.WriteLine("Token validate successfully!");
				return Task.CompletedTask;
			}
			
		};
	});

builder.Services.AddAuthorization();

// load printful settings
builder.Services.Configure<PrintfulConfig>(builder.Configuration.GetSection("Printful"));

// required for accessing httpcontext
builder.Services.AddHttpContextAccessor();

// Register services and reprositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPrintfulSyncService, PrintfulSyncService>();
builder.Services.AddScoped<IBaseProductRepository, BaseProductRepository>();
builder.Services.AddScoped<IBaseVariantRepository, BaseVariantRepository>();
builder.Services.AddScoped<IStoreProductRepository, StoreProductRepository>();
builder.Services.AddScoped<IStoreVariantRepository, StoreVariantRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IStoreVariantFilesRepository, StoreVariantFilesRepository>();
builder.Services.AddScoped<ICustomizableProductRepository, CustomizableProductRepository>();
builder.Services.AddScoped<ICustomizableVariantRepository, CustomizableVariantRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IAvailableRegionRepository, AvailableRegionRepository>();
builder.Services.AddScoped<ICustomizableProductService, CustomizableProductService>();

// Register Printful Integration Services
builder.Services.AddPrintfulServices();

// Register automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
