using System.Text.Json.Serialization;

namespace PrintfulIntegration.Models.PrintfulResponse;

public class PrintfulError
{
	[JsonPropertyName("message")]
	public string Message {  get; set; }

	[JsonPropertyName("reason")]
	public string Reason {  get; set; }
}
