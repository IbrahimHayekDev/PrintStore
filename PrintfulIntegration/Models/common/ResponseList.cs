namespace PrintfulIntegration.Models.common;

public class ResponseList<T> where T : class
{
	public ResponseList()
	{
		Items = new List<T>();
	}
	public List<T> Items { get; set; }
	public Paging Paging { get; set; }
}
