using Microsoft.AspNetCore.SignalR.Protocol;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrintfulIntegration.Models.common;

public class Response
{
	public Error Error { get; set; }
	public bool IsSuccessful => !Error.Errors.Any();
	public Response()
	{
		Error = new Error();
	}
}
public class Response<T> : Response
{
	public Response()
	{

	}
	public Response(T value)
	{
		Data = value;
		Error = new Error();
	}
	public Response(Error error)
	{
		Error = error;
	}

	public T Data { get; set; }
}
