namespace PrintStoreApi.Models.Products.Customizable;


public class ProductSizeGuideDTO
{
	public long ProductId { get; set; }
	public List<dynamic> AvailableSizes { get; set; }
	public List<SizeTableDTO> SizeTables { get; set; }
}

public class SizeTableDTO
{
	public string Type { get; set; }
	public string? Unit { get; set; }
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }
	public string? ImageDescription { get; set; }
	public List<MeasurementDTO> Measurements { get; set; }
}

public class MeasurementDTO
{
	public string? TypeLabel { get; set; }
	public string? Unit { get; set; }
	public List<MeasurementValueDTO> Values { get; set; }
}


public class MeasurementValueDTO
{
	public string Size { get; set; }
	public string? Value { get; set; }
	public string? MinValue { get; set; }
	public string? MaxValue { get; set; }
}
