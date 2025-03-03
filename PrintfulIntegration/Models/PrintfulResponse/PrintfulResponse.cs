
using System.Text.Json.Serialization;

namespace PrintfulIntegration.Models.PrintfulResponse;

public class PrintfulResponse<T>
{
	[JsonPropertyName("code")]
	public int Code { get; set;}

	[JsonPropertyName("result")]
	public T? Result { get; set;}

	[JsonPropertyName("paging")]
	public PrintfulPaging? Paging { get; set; }

	[JsonPropertyName("error")]
	public PrintfulError? Error { get; set; }

	[JsonPropertyName("extra")]
	public List<object>? Extra { get; set; }

}
