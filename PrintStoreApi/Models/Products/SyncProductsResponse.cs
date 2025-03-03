using PrintStoreApi.Core.Entities.Product;
using PrintStoreApi.Models.Common;

namespace PrintStoreApi.Models.Products;

public class SyncProductsResponse
{
	public BaseProductSyncResponse baseProduct { get; set;}
	public StoreProductSyncResponse storeProduct { get; set;}
	public List<BaseVariantSyncResponse> baseVariant { get; set;} = new List<BaseVariantSyncResponse>();
	public List<StoreVariantSyncResponse> storeVariant { get; set; } = new List<StoreVariantSyncResponse>();
}

public class BaseVariantSyncResponse : BaseVariant
{
	public SyncProductStatus SyncStatus {  get; set; }
	public List<difference> Changes { get; set; } = new List<difference>();
	public int dbID { get; set;}
}
public class StoreVariantSyncResponse : StoreVariant
{
	public SyncProductStatus SyncStatus { get; set; }
	public List<difference> Changes { get; set; } = new List<difference>();
	public int dbID { get; set; }
}
public class BaseProductSyncResponse : BaseProduct
{
	public SyncProductStatus SyncStatus { get; set; }
	public List<difference> Changes { get; set; } = new List<difference>();
	public int dbID { get; set; }
}
public class StoreProductSyncResponse : StoreProduct
{
	public SyncProductStatus SyncStatus { get; set; }
	public List<difference> Changes { get; set; } = new List<difference>();
	public int dbID { get; set; }
}
public class difference
{
	public string Name { get; set; }
	public object? OldValue { get; set; }
	public object? NewValue { get; set; }
}