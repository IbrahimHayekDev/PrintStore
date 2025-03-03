using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Models.Common;

public class Response
{
	public Error Error { get; set; }
	public bool IsSuccessful => !Error.Errors.Any();
	public Response()
	{
		Error = new Error();
	}
	public ApiMessage? apiMessage { get; set; }

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
