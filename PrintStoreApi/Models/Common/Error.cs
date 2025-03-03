using System.Text.Json.Serialization;

namespace PrintStoreApi.Models.Common;

public class Error
{
	public List<string> Errors { get; set; }
	public int ErrorCode { get; set; }

	[JsonIgnore]
	public Exception Exception { get; set; }

	public Error()
	{
		Errors = new List<string>();
	}
	public override string ToString()
	{
		return $"{DateTime.Now:dd-MM-yyyy hh:mm:ss t}{Environment.NewLine}Message: {Errors.Aggregate((x, y) => x + Environment.NewLine + y)}";
	}

	public class ErrorDetails
	{
		public string ErrorMessage { get; set; }
		public int ErrorCode { get; set; }
	}
}
