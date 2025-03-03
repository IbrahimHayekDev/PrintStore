namespace PrintStoreApi.Models.Common;

public class ListResponse<T> where T : class
{
	public ListResponse()
	{
		Items = new List<T>();
	}
	public List<T> Items { get; set; }
	public int TotalCount { get; set; }
}
