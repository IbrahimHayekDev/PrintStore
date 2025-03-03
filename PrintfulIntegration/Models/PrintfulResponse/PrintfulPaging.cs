using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PrintfulIntegration.Models.PrintfulResponse;

public class PrintfulPaging
{
	[JsonPropertyName("offset")]
	public int Offset { get; set; }

	[JsonPropertyName("limit")]
	public int Limit { get; set; }

	[JsonPropertyName("total")]
	public int Total { get; set; }
}
