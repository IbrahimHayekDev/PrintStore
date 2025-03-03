using PrintfulIntegration.Core.Interfaces.Services;

using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Core.Interfaces.Repositories.Products;
using PrintStoreApi.Core.Interfaces.Services;

using System.Globalization;
using PrintStoreApi.Models.Products;
using PrintStoreApi.Models.Common;
using System.Reflection;
using PrintfulIntegration.Services;
using PrintStoreApi.Repositories.Products;
using PrintStoreApi.Models.Products.Customizable;
using PrintfulIntegration.Models.Products.ProductCatalog;
using System.Diagnostics.Metrics;
using Newtonsoft.Json.Linq;
using System.Drawing;


namespace PrintStoreApi.Services.Product;

public class PrintfulSyncService: IPrintfulSyncService
{
	private readonly IPrintfulProductService _printfulProductService;
	private readonly IBaseProductRepository _baseProductRepository;
	private readonly IBaseVariantRepository _baseVariantRepository;
	private readonly IStoreProductRepository _storeProductRepository;
	private readonly IStoreVariantRepository _storeVariantRepository;
	private readonly IStoreVariantFilesRepository _storeVariantFilesRepository;
	private readonly IPrintfulCategotyService _printfulCategotyService;
	private readonly IProductCategoryRepository _productCategoryRepository;
	private readonly ICustomizableProductRepository _customizableProductRepository;
	private readonly ICustomizableVariantRepository _customizableVariantRepository;
	private readonly IAvailableRegionRepository _availableRegionRepository;
	private readonly IMaterialRepository _materialRepository;
	public PrintfulSyncService(
		IPrintfulProductService printfulProductService,
		IBaseProductRepository baseProductRepository,
		IBaseVariantRepository baseVariantRepository,
		IStoreProductRepository storeProductRepository,
		IStoreVariantRepository storeVariantRepository,
		IStoreVariantFilesRepository storeVariantFilesRepository,
		IPrintfulCategotyService printfulCategotyService,
		IProductCategoryRepository productCategoryRepository,
		 ICustomizableVariantRepository customizableVariantRepository,
		 ICustomizableProductRepository customizableProductRepository,
		 IMaterialRepository materialRepository,
		 IAvailableRegionRepository availableRegionRepository
		)
	{
		_printfulProductService = printfulProductService;
		_baseProductRepository = baseProductRepository;
		_baseVariantRepository = baseVariantRepository;
		_storeProductRepository = storeProductRepository;
		_storeVariantRepository = storeVariantRepository;
		_storeVariantFilesRepository = storeVariantFilesRepository;
		_printfulCategotyService = printfulCategotyService;
		_productCategoryRepository = productCategoryRepository;
		_customizableProductRepository = customizableProductRepository;
		_customizableVariantRepository = customizableVariantRepository;
		_materialRepository = materialRepository;
		_availableRegionRepository = availableRegionRepository;
	}

	public async Task<Response<string>> SyncCustomizableProducts()
	{
		var response = new Response<string>();
		var AddedProducts = 0;
		var AddedVariants = 0;
		var UpdatedProducts = 0;
		var UpdatedVariants = 0;
		var productListResponse = await _printfulProductService.GetAllBaseProducts();
		if (!productListResponse.IsSuccessful)
		{
			response.Error.Errors.Add("Failed to get all products from source.");
			return response;
		}
		productListResponse.Data.Items = productListResponse.Data.Items.Skip(350).Take(100).ToList();
		foreach (var product in productListResponse.Data.Items)
		{
			
			var productDb = await _customizableProductRepository.GetProductByPrintfulID(product.id);
			if(productDb == null )
			{
				// Add new product
				productDb = await _customizableProductRepository.AddAsync( new CustomizableProduct
				{
					PrintfulId = product.id,
					CategoryId = product.main_category_id,
					ProductType = product.type,
					ProductTypeName = product.type_name,
					ProductDescription = product.description,
					Title = product.title,
					Brand = product.brand,
					ImageUrl = product.image,
					Model = product.model,
					VariantCount = product.variant_count,
					Currency = product.currency,
					IsDiscontinued = product.is_discontinued == true ? 1 : 0,
					AverageFulfilmentTime = product.avg_fulfillment_time,
					OriginCountry = product.origin_country,
				});
				AddedProducts++;
			}
			else
			{
				//update product
				var hasChanges = false;
				if(productDb.ImageUrl != product.image)
				{
					productDb.ImageUrl = product.image;
					hasChanges = true;

				}
				if(productDb.IsDiscontinued != (product.is_discontinued == true ? 1 : 0))
				{
					productDb.IsDiscontinued = product.is_discontinued == true ? 1 : 0;
					hasChanges = true;
				}
				if (productDb.AverageFulfilmentTime != product.avg_fulfillment_time)
				{
					productDb.AverageFulfilmentTime = product.avg_fulfillment_time;
					hasChanges = true;
				}
				if (hasChanges)
				{
					await _customizableProductRepository.UpdateAsync(productDb);
					UpdatedProducts++;
				}
			}

			// Check variants
			var productDetailsResponse = await _printfulProductService.GetProductByIdAsync(product.id);
			if (!productDetailsResponse.IsSuccessful)
				continue;
			var variants = productDetailsResponse.Data.variants;
			foreach (var variant in variants)
			{
				var variantDB = await _customizableVariantRepository.GetVariantByPrintfulID(variant.id);
				if (variantDB == null)
				{
					// Add new variant
					variantDB = await _customizableVariantRepository.AddAsync(new CustomizableVarint
					{
						PrintfulId = variant.id,
						BaseProductId = productDb.Id,
						PrintfulProductId = variant.product_id,
						VariantName = variant.name,
						Color = variant.color,
						ColorCode1 = variant.color_code,
						ColorCode2 = variant.color_code2,
						ImageUrl = variant.image,
						Price = decimal.TryParse(variant.price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice) ? parsedPrice : 0,
						Size = variant.size,
						InStock = variant.in_stock == true ? 1 : 0,
					});
					AddedVariants++;
				}
				else
				{
					var hasChanges = false;
					//update variant
					if(variantDB.ImageUrl != variant.image)
					{
						variantDB.ImageUrl = variant.image;
						hasChanges = true;
					}
					if(variantDB.Price.ToString() != variant.price)
					{
						variantDB.Price = decimal.TryParse(variant.price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice) ? parsedPrice : 0;
						hasChanges = true;
					}
					if (variantDB.InStock != (variant.in_stock == true ? 1 : 0)){
						variantDB.InStock = variant.in_stock == true ? 1 : 0;
						hasChanges = true;
					}
					if (hasChanges)
					{
						variantDB = await _customizableVariantRepository.UpdateAsync(variantDB);
						UpdatedVariants++;
					}
				}
				foreach (var material in variant.material)
				{
					var materialDB = await _materialRepository.GetMaterialByPrintfulID(variant.id,material.name);
					if(materialDB == null)
					{
						//AddedProducts Material
						materialDB = await _materialRepository.AddAsync( new ProductMaterial
						{
							VariantId = variantDB.Id,
							MaterialName = material.name,
							MaterialPercentage = material.percentage
						});
					}
					else
					{
						//checked update material
						if(materialDB.MaterialPercentage != material.percentage)
						{
							materialDB.MaterialPercentage = material.percentage;
							await _materialRepository.UpdateAsync(materialDB);

						}
					}
		
				}

				foreach (var region in variant.availability_status)
				{
					var regionDB = await _availableRegionRepository.GetRegionByProductID(variant.id, region.region);
					if (regionDB == null)
					{
						//AddedProducts Region
						regionDB = await _availableRegionRepository.AddAsync(new AvailableRegion
						{
							VariantId = variantDB.Id,
							RegionName = region.region,
							AvailableStatus = region.status
						});
					}
					else
					{
						//checked update material
						if (regionDB.AvailableStatus != region.status)
						{
							regionDB.AvailableStatus = region.status;
							await _availableRegionRepository.UpdateAsync(regionDB);
							UpdatedVariants++;
						}
					}

				}
			}

		}
		response.Data = $"Added Products count: {AddedProducts} | Updated Products count: {UpdatedProducts} | Added Variants count: {AddedVariants} | Updated Variants count: {UpdatedVariants} ";
		return response;
	}
	public async Task<Response<ListResponse<SyncProductsResponse>>> GetProductsToSyncAsync()
	{
		var response = new Response<ListResponse<SyncProductsResponse>>();
		var listingResponse = new ListResponse<SyncProductsResponse>();
		var syncedProducts = new List<SyncProductsResponse>();
		var productListResponse = await _printfulProductService.GetStoreProductsAsync();
		if(!productListResponse.IsSuccessful) {
			response.Error.Errors.Add("Failed to get store products from source.");
			return response;
		}
		foreach(var storeProduct in productListResponse.Data.Items)
		{
			var singleSyncedProduct = new SyncProductsResponse();
			var storeProductDetailsResponse = await _printfulProductService.GetStoreProductByIdAsync(storeProduct.id.ToString());
			var productDetailsResponse = await _printfulProductService.GetProductByIdAsync(storeProductDetailsResponse.Data.sync_variants[0].product.product_id);
			if (!productDetailsResponse.IsSuccessful) 
				continue;
			// Check if baseproduct is synced with DB
			var baseProductDb = await _baseProductRepository.GetBaseProductByPrintfulID(productDetailsResponse.Data.product.id.ToString());
			var printfulBaseProduct = productDetailsResponse.Data.product;
			// Add baseproduct if it does not exist
			if (baseProductDb == null)
			{
				singleSyncedProduct.baseProduct = new BaseProductSyncResponse
				{
					PrintfulId = printfulBaseProduct.id,
					CategoryId = printfulBaseProduct.main_category_id,
					ProductType = printfulBaseProduct.type,
					ProductTypeName = printfulBaseProduct.type_name,
					ProductDescription = printfulBaseProduct.description,
					Title = printfulBaseProduct.title,
					Brand = printfulBaseProduct.brand,
					ImageUrl = printfulBaseProduct.image,
					Model = printfulBaseProduct.model,
					VariantCount = printfulBaseProduct.variant_count,
					Currency = printfulBaseProduct.currency,
					IsDiscontinued = printfulBaseProduct.is_discontinued == true ? 1 : 0,
					AverageFulfilmentTime = printfulBaseProduct.avg_fulfillment_time,
					OriginCountry = printfulBaseProduct.origin_country,
					SyncStatus = SyncProductStatus.New,
				};
				//baseProduct = await _baseProductRepository.AddAsync(baseProduct);
			}
			else
			{
				var printfulBaseProductMapped = new BaseProduct
				{
					PrintfulId = printfulBaseProduct.id,
					CategoryId = printfulBaseProduct.main_category_id,
					ProductType = printfulBaseProduct.type,
					ProductTypeName = printfulBaseProduct.type_name,
					ProductDescription = printfulBaseProduct.description,
					Title = printfulBaseProduct.title,
					Brand = printfulBaseProduct.brand,
					ImageUrl = printfulBaseProduct.image,
					Model = printfulBaseProduct.model,
					VariantCount = printfulBaseProduct.variant_count,
					Currency = printfulBaseProduct.currency,
					IsDiscontinued = printfulBaseProduct.is_discontinued == true ? 1 : 0,
					AverageFulfilmentTime = printfulBaseProduct.avg_fulfillment_time,
					OriginCountry = printfulBaseProduct.origin_country,
				};
				var objectDifferences = GetDifferences(baseProductDb, printfulBaseProductMapped, ["Id", "BaseProduct", "BaseVariant", "StoreProduct", "StoreProductId", "BaseProductId", "Files"]);
				singleSyncedProduct.baseProduct = new BaseProductSyncResponse
				{
					PrintfulId = printfulBaseProduct.id,
					CategoryId = printfulBaseProduct.main_category_id,
					ProductType = printfulBaseProduct.type,
					ProductTypeName = printfulBaseProduct.type_name,
					ProductDescription = printfulBaseProduct.description,
					Title = printfulBaseProduct.title,
					Brand = printfulBaseProduct.brand,
					ImageUrl = printfulBaseProduct.image,
					Model = printfulBaseProduct.model,
					VariantCount = printfulBaseProduct.variant_count,
					Currency = printfulBaseProduct.currency,
					IsDiscontinued = printfulBaseProduct.is_discontinued == true ? 1 : 0,
					AverageFulfilmentTime = printfulBaseProduct.avg_fulfillment_time,
					OriginCountry = printfulBaseProduct.origin_country,
					dbID = baseProductDb.Id,
					SyncStatus = 0,
				};
				if (objectDifferences.Count == 0)
				{
					// No changes between the object in the DB and the object synced
					singleSyncedProduct.baseProduct.SyncStatus = SyncProductStatus.Identical;
				}
				else
				{
					// These is changes between the object in the DB and the object synced
					singleSyncedProduct.baseProduct.SyncStatus = SyncProductStatus.HasChanges;
					singleSyncedProduct.baseProduct.Changes = objectDifferences;
				}
			}

			// Check if storeProduct is synced with DB
			var storeProductDb = await _storeProductRepository.GetStoreProductByPrintfulID(storeProduct.id.ToString());
			// Add storeProduct if it does not exist
			if(storeProductDb == null)
			{
				singleSyncedProduct.storeProduct = new StoreProductSyncResponse
				{
					PrintfulId = storeProduct.id,
					BaseProductId = singleSyncedProduct.baseProduct.dbID,
					ExternalID = storeProduct.external_id,
					ProductName = storeProduct.name,
					VariantCount = storeProduct.variants,
					SyncedVariantCount = storeProduct.synced,
					ThumbnailUrl = storeProduct.thumbnail_url,
					IsIgnored = storeProduct.is_ignored == true ? 1 : 0,
					CategoryId = singleSyncedProduct.baseProduct.CategoryId,
					SyncStatus = SyncProductStatus.New,
				};
				//storeProductDb = await _storeProductRepository.AddAsync(storeProductDb);
			}
			else
			{
				var printfulStoreProductMapped = new StoreProduct
				{
					PrintfulId = storeProduct.id,
					BaseProductId = singleSyncedProduct.baseProduct.dbID,
					ExternalID = storeProduct.external_id,
					ProductName = storeProduct.name,
					VariantCount = storeProduct.variants,
					SyncedVariantCount = storeProduct.synced,
					ThumbnailUrl = storeProduct.thumbnail_url,
					IsIgnored = storeProduct.is_ignored == true ? 1 : 0,
				};
				var objectDifferences = GetDifferences(storeProductDb, printfulStoreProductMapped, ["Id", "BaseProduct", "BaseVariant", "StoreProduct", "StoreProductId", "BaseProductId", "Files", "CategoryId"]);
				singleSyncedProduct.storeProduct = new StoreProductSyncResponse
				{
					PrintfulId = storeProduct.id,
					BaseProductId = singleSyncedProduct.baseProduct.dbID,
					ExternalID = storeProduct.external_id,
					ProductName = storeProduct.name,
					VariantCount = storeProduct.variants,
					SyncedVariantCount = storeProduct.synced,
					ThumbnailUrl = storeProduct.thumbnail_url,
					IsIgnored = storeProduct.is_ignored == true ? 1 : 0,
					dbID = storeProductDb.Id,
					SyncStatus = 0,
				};
				if (objectDifferences.Count == 0)
				{
					// No changes between the object in the DB and the object synced
					singleSyncedProduct.storeProduct.SyncStatus = SyncProductStatus.Identical;
				}
				else
				{
					// These is changes between the object in the DB and the object synced
					singleSyncedProduct.storeProduct.SyncStatus = SyncProductStatus.HasChanges;
					singleSyncedProduct.storeProduct.Changes = objectDifferences;
				}
			}

			// Sync base variants
			var printfulBaseVariants = productDetailsResponse.Data.variants;
			var printfulStoreVariants = storeProductDetailsResponse.Data.sync_variants;
			foreach (var variant in printfulBaseVariants)
			{
				// Check if baseVariant is synced with DB
				var baseVariantDb = await _baseVariantRepository.GetBaseVariantByPrintfulID(variant.id.ToString());
				// Add baseVariant if it does not exist
				if(baseVariantDb == null) {
					singleSyncedProduct.baseVariant.Add( new BaseVariantSyncResponse
					{
						PrintfulId = variant.id,
						BaseProductId = singleSyncedProduct.baseProduct.dbID,
						PrintfulProductId = singleSyncedProduct.baseProduct.PrintfulId,
						VariantName = variant.name,
						Color = variant.color,
						ColorCode1 = variant.color_code,
						ColorCode2 = variant.color_code2,
						ImageUrl = variant.image,
						Price = decimal.TryParse(variant.price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice) ? parsedPrice : 0,
						Size = variant.size,
						InStock = variant.in_stock == true ? 1 : 0,
						SyncStatus = SyncProductStatus.New,
					});
					//baseVariantDb = await _baseVariantRepository.AddAsync(baseVariantDb);
				}
				else
				{
					var printfulBaseVariantMapped = new BaseVariant
					{
						PrintfulId = variant.id,
						BaseProductId = singleSyncedProduct.baseProduct.dbID,
						PrintfulProductId = singleSyncedProduct.baseProduct.PrintfulId,
						VariantName = variant.name,
						Color = variant.color,
						ColorCode1 = variant.color_code,
						ColorCode2 = variant.color_code2,
						ImageUrl = variant.image,
						Price = decimal.TryParse(variant.price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice) ? parsedPrice : 0,
						Size = variant.size,
						InStock = variant.in_stock == true ? 1 : 0,
					};
					var objectDifferences = GetDifferences(baseVariantDb, printfulBaseVariantMapped, ["Id", "BaseProduct", "BaseVariant", "StoreProduct", "StoreProductId", "BaseProductId", "Files"]);
					var variantToAdd = new BaseVariantSyncResponse();
					if (objectDifferences.Count == 0)
					{
						// No changes between the object in the DB and the object synced
						variantToAdd = new BaseVariantSyncResponse
						{
							PrintfulId = variant.id,
							BaseProductId = singleSyncedProduct.baseProduct.dbID,
							PrintfulProductId = singleSyncedProduct.baseProduct.PrintfulId,
							VariantName = variant.name,
							Color = variant.color,
							ColorCode1 = variant.color_code,
							ColorCode2 = variant.color_code2,
							ImageUrl = variant.image,
							Price = decimal.TryParse(variant.price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsePrice) ? parsePrice : 0,
							Size = variant.size,
							InStock = variant.in_stock == true ? 1 : 0,
							SyncStatus = SyncProductStatus.Identical,
							dbID = baseVariantDb.Id
						};
					}
					else
					{
						// These is changes between the object in the DB and the object synced
						variantToAdd = new BaseVariantSyncResponse
						{
							PrintfulId = variant.id,
							BaseProductId = singleSyncedProduct.baseProduct.Id,
							PrintfulProductId = singleSyncedProduct.baseProduct.PrintfulId,
							VariantName = variant.name,
							Color = variant.color,
							ColorCode1 = variant.color_code,
							ColorCode2 = variant.color_code2,
							ImageUrl = variant.image,
							Price = decimal.TryParse(variant.price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsePrice) ? parsePrice : 0,
							Size = variant.size,
							InStock = variant.in_stock == true ? 1 : 0,
							SyncStatus = SyncProductStatus.HasChanges,
							Changes = objectDifferences,
							dbID = baseVariantDb.Id

						};
					}
					singleSyncedProduct.baseVariant.Add(variantToAdd);
				}
				// loop store product variants
				var storeProductVariants = printfulStoreVariants.Where(v => v.variant_id == variant.id).ToList();
				foreach (var storeVariant in storeProductVariants)
				{
					var storeVariantDB = await _storeVariantRepository.GetStoreVariantByPrintfulID(storeVariant.id.ToString());
					
					if (storeVariantDB == null)
					{
						//storeVariantDB = new StoreVariant
						singleSyncedProduct.storeVariant.Add( new StoreVariantSyncResponse
						{
							PrintfulId = storeVariant.id,
							BaseProductId = singleSyncedProduct.baseProduct.dbID,
							StoreProductId = singleSyncedProduct.storeProduct.dbID,
							BaseVariantId = baseVariantDb != null ? baseVariantDb.Id : 0,
							ExternalID = storeVariant.external_id,
							PrintfulStoreProductId = singleSyncedProduct.storeProduct.PrintfulId,
							PrintfulVariantId = variant.id,
							AvailabilityStatus = storeVariant.availability_status,
							Color = storeVariant.color,
							Size = storeVariant.size,
							Currency = storeVariant.currency,
							SKU = storeVariant.sku,
							VariantName = storeVariant.name,
							RetailPrice = decimal.TryParse(storeVariant.retail_price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice) ? parsedPrice : 0,
							CategoryId = storeVariant.main_category_id != null ? storeVariant.main_category_id : 1000,
							IsIgnored = storeVariant.is_ignored == true ? 1 : 0,
							IsSynced = storeVariant.synced == true ? 1 : 0,
							Files = storeVariant.files.Where(file => file.type =="preview").Select(file => new VariantFile
							{
								PrintfultId=file.id,
								NameFile = file.filename,
								MimeType = file.mime_type,
								CreatedAt = file.created,
								ThumbnailUrl = file.thumbnail_url,
								PreviewUrl = file.preview_url,
								VariantId = storeVariantDB != null ? storeVariantDB.Id : 0,
							}).ToList(),
							SyncStatus = SyncProductStatus.New,
						});
						//storeVariantDB = await _storeVariantRepository.AddAsync(storeVariantDB);
					}
					else
					{
						var printfulStoreVariantMapped = new StoreVariant
						{
							PrintfulId = storeVariant.id,
							BaseProductId = singleSyncedProduct.baseProduct.dbID,
							StoreProductId = singleSyncedProduct.storeProduct.dbID,
							BaseVariantId = baseVariantDb != null ? baseVariantDb.Id : 0,
							ExternalID = storeVariant.external_id,
							PrintfulStoreProductId = singleSyncedProduct.storeProduct.PrintfulId,
							PrintfulVariantId = variant.id,
							AvailabilityStatus = storeVariant.availability_status,
							Color = storeVariant.color,
							Size = storeVariant.size,
							Currency = storeVariant.currency,
							SKU = storeVariant.sku,
							VariantName = storeVariant.name,
							RetailPrice = decimal.TryParse(storeVariant.retail_price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice) ? parsedPrice : 0,
							CategoryId = storeVariant.main_category_id != null ? storeVariant.main_category_id : 1000,
							IsIgnored = storeVariant.is_ignored == true ? 1 : 0,
							IsSynced = storeVariant.synced == true ? 1 : 0,
							Files = storeVariant.files.Where(file => file.type == "preview").Select(file => new VariantFile
							{
								PrintfultId = file.id,
								NameFile = file.filename,
								MimeType = file.mime_type,
								CreatedAt = file.created,
								ThumbnailUrl = file.thumbnail_url,
								PreviewUrl = file.preview_url,
								VariantId = storeVariantDB.Id,
							}).ToList(),
						};
						var objectDifferences = GetDifferences(storeVariantDB, printfulStoreVariantMapped, ["Id", "BaseProduct", "BaseVariant", "StoreProduct", "StoreProductId", "BaseProductId", "Files"]);
						var storeVariantToAdd = new StoreVariantSyncResponse();
						if (objectDifferences.Count == 0)
						{
							// No changes between the object in the DB and the object synced
							storeVariantToAdd = new StoreVariantSyncResponse
							{
								PrintfulId = storeVariant.id,
								BaseProductId = singleSyncedProduct.baseProduct.dbID,
								StoreProductId = singleSyncedProduct.storeProduct.dbID,
								BaseVariantId = baseVariantDb != null ? baseVariantDb.Id : 0,
								ExternalID = storeVariant.external_id,
								PrintfulStoreProductId = singleSyncedProduct.storeProduct.PrintfulId,
								PrintfulVariantId = variant.id,
								AvailabilityStatus = storeVariant.availability_status,
								Color = storeVariant.color,
								Size = storeVariant.size,
								Currency = storeVariant.currency,
								SKU = storeVariant.sku,
								VariantName = storeVariant.name,
								RetailPrice = decimal.TryParse(storeVariant.retail_price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice2) ? parsedPrice2 : 0,
								CategoryId = storeVariant.main_category_id != null ? storeVariant.main_category_id : 1000,
								IsIgnored = storeVariant.is_ignored == true ? 1 : 0,
								IsSynced = storeVariant.synced == true ? 1 : 0,
								SyncStatus = SyncProductStatus.Identical,
								dbID = storeVariantDB.Id,
								Files = storeVariant.files.Where(file => file.type == "preview").Select(file => new VariantFile
								{
									PrintfultId = file.id,
									NameFile = file.filename,
									MimeType = file.mime_type,
									CreatedAt = file.created,
									ThumbnailUrl = file.thumbnail_url,
									PreviewUrl = file.preview_url,
									VariantId = storeVariantDB.Id,
								}).ToList(),
							};
						}
						else
						{
							// These is changes between the object in the DB and the object synced
							storeVariantToAdd = new StoreVariantSyncResponse
							{
								PrintfulId = storeVariant.id,
								BaseProductId = singleSyncedProduct.baseProduct.dbID,
								StoreProductId = singleSyncedProduct.storeProduct.dbID,
								BaseVariantId = baseVariantDb != null ? baseVariantDb.Id : 0,
								ExternalID = storeVariant.external_id,
								PrintfulStoreProductId = singleSyncedProduct.storeProduct.PrintfulId,
								PrintfulVariantId = variant.id,
								AvailabilityStatus = storeVariant.availability_status,
								Color = storeVariant.color,
								Size = storeVariant.size,
								Currency = storeVariant.currency,
								SKU = storeVariant.sku,
								VariantName = storeVariant.name,
								RetailPrice = decimal.TryParse(storeVariant.retail_price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice3) ? parsedPrice3 : 0,
								CategoryId = storeVariant.main_category_id != null ? storeVariant.main_category_id : 1000,
								IsIgnored = storeVariant.is_ignored == true ? 1 : 0,
								IsSynced = storeVariant.synced == true ? 1 : 0,
								Changes = objectDifferences,
								SyncStatus = SyncProductStatus.HasChanges,
								dbID = storeVariantDB.Id,
								Files = storeVariant.files.Where(file => file.type == "preview").Select(file => new VariantFile
								{
									PrintfultId = file.id,
									NameFile = file.filename,
									MimeType = file.mime_type,
									CreatedAt = file.created,
									ThumbnailUrl = file.thumbnail_url,
									PreviewUrl = file.preview_url,
									VariantId = storeVariantDB.Id,
								}).ToList(),
							};
						}
						singleSyncedProduct.storeVariant.Add(storeVariantToAdd);
					}
				}
			}
			syncedProducts.Add(singleSyncedProduct);
		}
		listingResponse.Items = syncedProducts;
		listingResponse.TotalCount = syncedProducts.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<bool>> SyncSingleProductAsync(SyncProductsResponse product)
	{
		var response = new Response<bool>();
		var baseProduct = new BaseProduct();
		var baseProductRespose = product.baseProduct;
		//var baseVariant = new BaseVariant();
		var baseVariantRespose = product.baseVariant;
		var storeProduct = new StoreProduct();
		var storeProductRespose = product.storeProduct;
		//var storeVariant = new StoreVariant();
		var storeVariantRespose = product.storeVariant;

		if (baseProductRespose == null || storeProductRespose == null)
		{
			response.Error.Errors.Add("Either baseProduct OR  storeProduct are null");
			response.Data = false;
			return response;
		}
		//Base Product
		baseProduct = new BaseProduct
		{
			PrintfulId = baseProductRespose.PrintfulId,
			CategoryId = baseProductRespose.CategoryId,
			ProductType = baseProductRespose.ProductType,
			ProductTypeName = baseProductRespose.ProductTypeName,
			ProductDescription = baseProductRespose.ProductDescription,
			Title = baseProductRespose.Title,
			Brand = baseProductRespose.Brand,
			ImageUrl = baseProductRespose.ImageUrl,
			Model = baseProductRespose.Model,
			VariantCount = baseProductRespose.VariantCount,
			Currency = baseProductRespose.Currency,
			IsDiscontinued = baseProductRespose.IsDiscontinued,
			AverageFulfilmentTime = baseProductRespose.AverageFulfilmentTime,
			OriginCountry = baseProductRespose.OriginCountry,
		};
		if (baseProductRespose.dbID == 0)
		{
			// base product is new | Add base product
			baseProduct = await _baseProductRepository.AddAsync(baseProduct);
		}
		else
		{
			var baseProductToUpdate = await _baseProductRepository.GetByIdAsync(baseProductRespose.dbID);
			baseProductToUpdate.PrintfulId = baseProduct.PrintfulId;
			baseProductToUpdate.CategoryId = baseProduct.CategoryId;
			baseProductToUpdate.ProductType = baseProduct.ProductType;
			baseProductToUpdate.ProductTypeName = baseProduct.ProductTypeName;
			baseProductToUpdate.ProductDescription = baseProduct.ProductDescription;
			baseProductToUpdate.Title = baseProduct.Title;
			baseProductToUpdate.Brand = baseProduct.Brand;
			baseProductToUpdate.ImageUrl = baseProduct.ImageUrl;
			baseProductToUpdate.Model = baseProduct.Model;
			baseProductToUpdate.VariantCount = baseProduct.VariantCount;
			baseProductToUpdate.Currency = baseProduct.Currency;
			baseProductToUpdate.IsDiscontinued = baseProduct.IsDiscontinued;
			baseProductToUpdate.AverageFulfilmentTime = baseProduct.AverageFulfilmentTime;
			baseProductToUpdate.OriginCountry = baseProduct.OriginCountry;
			// update Base product 
			baseProduct = await _baseProductRepository.UpdateAsync(baseProductToUpdate);
		}

		//Store Product
		storeProduct = new StoreProduct
		{
			PrintfulId = storeProductRespose.PrintfulId,
			BaseProductId = baseProduct.Id,
			ExternalID = storeProductRespose.ExternalID,
			ProductName = storeProductRespose.ProductName,
			VariantCount = storeProductRespose.VariantCount,
			SyncedVariantCount = storeProductRespose.SyncedVariantCount,
			ThumbnailUrl = storeProductRespose.ThumbnailUrl,
			IsIgnored = storeProductRespose.IsIgnored,
			CategoryId = baseProduct.CategoryId
		};
		if (storeProductRespose.dbID == 0)
		{
			// base product is new | Add base product
			storeProduct = await _storeProductRepository.AddAsync(storeProduct);
		}
		else
		{
			var storeProductToUpdate = await _storeProductRepository.GetByIdAsync(storeProductRespose.dbID);
			storeProductToUpdate.PrintfulId = storeProduct.PrintfulId;
			storeProductToUpdate.ExternalID = storeProduct.ExternalID;
			storeProductToUpdate.ProductName = storeProduct.ProductName;
			storeProductToUpdate.VariantCount = storeProduct.VariantCount;
			storeProductToUpdate.SyncedVariantCount = storeProduct.SyncedVariantCount;
			storeProductToUpdate.ThumbnailUrl = storeProduct.ThumbnailUrl;
			storeProductToUpdate.IsIgnored = storeProduct.IsIgnored;
			storeProductToUpdate.CategoryId = storeProduct.CategoryId;
			// update Base product 
			storeProduct = await _storeProductRepository.UpdateAsync(storeProductToUpdate);
		}

		//Base Variant
		foreach (var baseVariant in baseVariantRespose)
		{
			var _baseVariant = new BaseVariant
			{
				PrintfulId = baseVariant.PrintfulId,
				BaseProductId = baseProduct.Id,
				PrintfulProductId = baseVariant.PrintfulProductId,
				VariantName = baseVariant.VariantName,
				Color = baseVariant.Color,
				ColorCode1 = baseVariant.ColorCode1,
				ColorCode2 = baseVariant.ColorCode2,
				ImageUrl = baseVariant.ImageUrl,
				Price = baseVariant.Price,
				Size = baseVariant.Size,
				InStock = baseVariant.InStock,
			};
			if (baseVariant.dbID == 0)
			{
				// Base variant is new | Add Base variant
				_baseVariant = await _baseVariantRepository.AddAsync(_baseVariant);
			}
			else
			{
				var baseVariantToUpdate = await _baseVariantRepository.GetByIdAsync(baseVariant.dbID);
				baseVariantToUpdate.PrintfulId = _baseVariant.PrintfulId;
				baseVariantToUpdate.PrintfulProductId = _baseVariant.PrintfulProductId;
				baseVariantToUpdate.VariantName = _baseVariant.VariantName;
				baseVariantToUpdate.Color = _baseVariant.Color;
				baseVariantToUpdate.ColorCode1 = _baseVariant.ColorCode1;
				baseVariantToUpdate.ColorCode2 = _baseVariant.ColorCode2;
				baseVariantToUpdate.ImageUrl = _baseVariant.ImageUrl;
				baseVariantToUpdate.Price = _baseVariant.Price;
				baseVariantToUpdate.Size = _baseVariant.Size;
				baseVariantToUpdate.InStock = _baseVariant.InStock;
				// update Base variant 
				_baseVariant = await _baseVariantRepository.UpdateAsync(baseVariantToUpdate);
			}

			// Store Variant
			var storeProductVariants = storeVariantRespose.Where(v => v.PrintfulVariantId == _baseVariant.PrintfulId).ToList();
			foreach (var storeVariant in storeProductVariants)
			{
				var _storeVariant = new StoreVariant
				{
					PrintfulId = storeVariant.PrintfulId,
					BaseProductId = baseProduct.Id,
					StoreProductId = storeProduct.Id,
					BaseVariantId = _baseVariant.Id,
					ExternalID = storeVariant.ExternalID,
					PrintfulStoreProductId = storeVariant.PrintfulStoreProductId,
					PrintfulVariantId = storeVariant.PrintfulVariantId,
					AvailabilityStatus = storeVariant.AvailabilityStatus,
					Color = storeVariant.Color,
					Size = storeVariant.Size,
					Currency = storeVariant.Currency,
					SKU = storeVariant.SKU,
					VariantName = storeVariant.VariantName,
					RetailPrice = storeVariant.RetailPrice,
					CategoryId = storeVariant.CategoryId,
					IsIgnored = storeVariant.IsIgnored,
					IsSynced = storeVariant.IsSynced,
				};
				if (storeVariant.dbID == 0)
				{
					// Store variant is new | Add Store variant
					_storeVariant = await _storeVariantRepository.AddAsync(_storeVariant);
					if (storeVariant.Files != null)
					{
						foreach (var file in storeVariant.Files)
						{
							await _storeVariantFilesRepository.AddAsync(new VariantFile
							{
								PrintfultId = file.PrintfultId,
								NameFile = file.NameFile,
								MimeType = file.MimeType,
								CreatedAt = file.CreatedAt,
								ThumbnailUrl = file.ThumbnailUrl,
								PreviewUrl = file.PreviewUrl,
								VariantId = _storeVariant.Id,
							});
						}
					}
				}
				else
				{
					var storeVariantToUpdate = await _storeVariantRepository.GetByIdAsync(storeVariant.dbID);
					storeVariantToUpdate.PrintfulId = _storeVariant.PrintfulId;
					storeVariantToUpdate.ExternalID = _storeVariant.ExternalID;
					storeVariantToUpdate.PrintfulStoreProductId = _storeVariant.PrintfulStoreProductId;
					storeVariantToUpdate.PrintfulVariantId = _storeVariant.PrintfulVariantId;
					storeVariantToUpdate.AvailabilityStatus = _storeVariant.AvailabilityStatus;
					storeVariantToUpdate.Color = _storeVariant.Color;
					storeVariantToUpdate.Size = _storeVariant.Size;
					storeVariantToUpdate.Currency = _storeVariant.Currency;
					storeVariantToUpdate.SKU = _storeVariant.SKU;
					storeVariantToUpdate.VariantName = _storeVariant.VariantName;
					storeVariantToUpdate.RetailPrice = _storeVariant.RetailPrice;
					storeVariantToUpdate.CategoryId = _storeVariant.CategoryId;
					storeVariantToUpdate.IsIgnored = _storeVariant.IsIgnored;
					storeVariantToUpdate.IsSynced = _storeVariant.IsSynced;
					// update Store variant 
					var updatedVariant = await _storeVariantRepository.UpdateAsync(storeVariantToUpdate);
					_storeVariant = await _storeVariantRepository.GetStoreVariantWithFiles(updatedVariant.Id);

					foreach (var integratedFile in storeVariant.Files)
					{
						if (_storeVariant.Files != null && _storeVariant.Files.Count > 0)
						{
							var dbFile = _storeVariant.Files.Where(dbF => dbF.PrintfultId == integratedFile.PrintfultId).FirstOrDefault();
							if (dbFile != null)
							{
								// update file
								var fileToUpdate = new VariantFile();
								fileToUpdate = dbFile;
								fileToUpdate.PrintfultId = integratedFile.PrintfultId;
								fileToUpdate.NameFile = integratedFile.NameFile;
								fileToUpdate.MimeType = integratedFile.MimeType;
								fileToUpdate.CreatedAt = integratedFile.CreatedAt;
								fileToUpdate.ThumbnailUrl = integratedFile.ThumbnailUrl;
								fileToUpdate.PreviewUrl = integratedFile.PreviewUrl;
								fileToUpdate.VariantId = dbFile.VariantId;
								await _storeVariantFilesRepository.UpdateAsync(fileToUpdate);
							}
						}
						else
						{
							//add file
							var fileToAdd = new VariantFile
							{
								PrintfultId = integratedFile.PrintfultId,
								NameFile = integratedFile.NameFile,
								MimeType = integratedFile.MimeType,
								ThumbnailUrl = integratedFile.ThumbnailUrl,
								PreviewUrl = integratedFile.PreviewUrl,
								VariantId = _storeVariant.Id,
							};

							await _storeVariantFilesRepository.AddAsync(fileToAdd);
						}
					}
				}
			}
		}
		response.Data = true;
		return response;
	}


	public async Task<Response<ListResponse<ProductCategoryDTO>>> GetPrintfulCategories()
	{
		var response = new Response<ListResponse<ProductCategoryDTO>>();
		var listingResponse = new ListResponse<ProductCategoryDTO>();
		var categories = new List<ProductCategoryDTO>();
		var categoriesListResponse = await _printfulCategotyService.GetPrintfulCategories();
		foreach (var category in categoriesListResponse.Data.categories)
		{
			categories.Add(new ProductCategoryDTO
			{
				PrintfulId = category.id,
				ParentPrintfulId = category.parent_id,
				ImageUrl = category.image_url,
				Size = category.size,
				Title = category.title,

			});
		}
		listingResponse.Items = categories;
		listingResponse.TotalCount = categories.Count();
		response.Data = listingResponse;
		return response;
	}

	public async Task<Response<bool>> SyncSingleCategory(ProductCategorySyncDTO categoryToSync)
	{
		var response = new Response<bool>();
		await SyncCategory(categoryToSync);
		await MapParentsID();
		response.Data = true;
		return response;
	}

	public async Task<Response<bool>> SyncCategoriesWithDB()
	{
		var response = new Response<bool>();
		var categoriesWithChangesRes = await GetCategoriesToSync();
		var categoriesWithChanges = categoriesWithChangesRes.Data.Items;
		foreach (var categoryChange in categoriesWithChanges)
		{
			await SyncCategory(categoryChange);
		}
		await MapParentsID();
		response.Data = true;
		return response;
	}

	public async Task<Response<ListResponse<ProductCategorySyncDTO>>> GetCategoriesToSync()
	{
		var response = new Response<ListResponse<ProductCategorySyncDTO>>();
		var listingResponse = new ListResponse<ProductCategorySyncDTO>();
		var categories = new List<ProductCategorySyncDTO>();
		var categoriesList = await _printfulCategotyService.GetPrintfulCategories();
		foreach (var category in categoriesList.Data.categories)
		{
			var categoryDB = await _productCategoryRepository.GetCategoryByPrintfulID(category.id);
			if (categoryDB == null)
			{
				// New Category
				categories.Add(new ProductCategorySyncDTO
				{
					PrintfulId = category.id,
					ParentPrintfulId = category.parent_id,
					parantTitle = categoriesList.Data.categories.FirstOrDefault(item => item.id == category.parent_id)?.title,
					ImageUrl = category.image_url,
					Size = category.size,
					Title = category.title,
					SyncStatus = SyncProductStatus.New,
					dbID = 0
				});
			}
			else
			{
				// Category exists | Check for changes
				var categoryToCheckDifference = new ProductCategory
				{
					PrintfulId = category.id,
					ParentPrintfulId = category.parent_id,
					ImageUrl = category.image_url,
					Size = category.size,
					Title = category.title
				};
				var objectDifferences = GetDifferences(categoryDB, categoryToCheckDifference, ["Id", "ParentId", "ParentCategory", "SubCategories", "BaseProducts", "StoreVariants", "CustomizableProduct"]);
				if (objectDifferences.Count > 0)
				{
					// These is changes between the object in the DB and the object synced
					categories.Add(new ProductCategorySyncDTO
					{
						PrintfulId = category.id,
						ParentPrintfulId = category.parent_id,
						parantTitle = categoriesList.Data.categories.FirstOrDefault(item => item.id == category.parent_id)?.title,
						ImageUrl = category.image_url,
						Size = category.size,
						Title = category.title,
						SyncStatus = SyncProductStatus.HasChanges,
						Changes = objectDifferences,
						dbID = categoryDB.Id
					});
				}
			}
		}
		listingResponse.Items = categories;
		response.Data = listingResponse;
		return response;
	}
	private async Task MapParentsID()
	{
		// mapping parent id after syncing
		var allCategoriesDb = await _productCategoryRepository.GetAllAsync();
		foreach (var item in allCategoriesDb)
		{
			if (item.ParentPrintfulId != null && item.ParentPrintfulId != 0)
			{
				var parentCategory = await _productCategoryRepository.GetCategoryByPrintfulID(item.ParentPrintfulId);
				var parentID = parentCategory != null ? parentCategory.Id : 0;
				item.ParentId = parentID;
				await _productCategoryRepository.UpdateAsync(item);
			}
		}
	}

	private async Task SyncCategory(ProductCategorySyncDTO categoryChange)
	{
		if (categoryChange.SyncStatus == SyncProductStatus.New)
		{
			await _productCategoryRepository.AddAsync(new ProductCategory
			{
				Size = categoryChange.Size,
				Title = categoryChange.Title,
				ParentPrintfulId = categoryChange.ParentPrintfulId,
				ImageUrl = categoryChange.ImageUrl,
				PrintfulId = categoryChange.PrintfulId
			});
		}
		else
		{
			var categoryToUpdate = await _productCategoryRepository.GetByIdAsync(categoryChange.dbID);
			if (categoryToUpdate != null)
			{
				categoryToUpdate.Size = categoryChange.Size;
				categoryToUpdate.Title = categoryChange.Title;
				categoryToUpdate.ParentPrintfulId = categoryChange.ParentPrintfulId;
				categoryToUpdate.ImageUrl = categoryChange.ImageUrl;
				categoryToUpdate.PrintfulId = categoryChange.PrintfulId;
				await _productCategoryRepository.UpdateAsync(categoryToUpdate);
			}
		}
	}

	private List<difference> GetDifferences<T>(T obj1, T obj2, List<string> _exceptions)
	{
		List<difference> differences = new();
		if (obj1 == null || obj2 == null)
		{
			return differences;
		}
		PropertyInfo[] properties = typeof(T).GetProperties();
		var exceptions = new List<string>(_exceptions);
		foreach (PropertyInfo property in properties)
		{
			if (exceptions.Contains(property.Name))
				continue;
			object value1 = property.GetValue(obj1);
			object value2 = property.GetValue(obj2);
			if (!Equals(value1, value2))
			{
				differences.Add(new difference
				{
					Name = property.Name,
					OldValue = value1,
					NewValue = value2
				});
			}
		}
		return differences;
	}

	public async Task<Response<ProductSizeGuideDTO>> ProductSizeGuideByPIdAsync(long productId)
	{
		var response = new Response<ProductSizeGuideDTO>();
		var sizeGuideResponse = await _printfulProductService.GetSizeGuideByProductId(productId);
		if (!sizeGuideResponse.IsSuccessful)
		{
			response.Error.Errors.Add("Failed to get store products from source.");
			return response;
		}

		var productSizeGuid = new ProductSizeGuideDTO
		{
			ProductId = sizeGuideResponse.Data.product_id,
			AvailableSizes = sizeGuideResponse.Data.available_sizes,
			SizeTables = sizeGuideResponse.Data.size_tables?.Select(MapSizeTable).ToList()
		};
		response.Data = productSizeGuid;
		return response;

	}

	private static SizeTableDTO MapSizeTable(SizeTable st) {
		return new SizeTableDTO {
			Type = st.type,
			Unit = st.unit,
			Description = st.description,
			ImageUrl = st.image_url,
			ImageDescription = st.image_description,
			Measurements = st.measurements?.Select(MapMeasures).ToList(),
		};
	}
	private static MeasurementDTO MapMeasures(Measurement itm)
	{
		return new MeasurementDTO
		{
			TypeLabel = itm.type_label,
			Unit = itm.unit,
			Values = itm.values?.Select(MapValues).ToList(),
		};
	}
	private static MeasurementValueDTO MapValues(MeasurementValue itm)
	{
		return new MeasurementValueDTO
		{
			Size = itm.size,
			Value = itm.value,
			MinValue = itm.min_value,
			MaxValue = itm.max_value,
		};
	}
}
