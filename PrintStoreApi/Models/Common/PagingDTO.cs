using System.Runtime.Serialization;

namespace PrintStoreApi.Models.Common;

public class PagingDTO
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	[IgnoreDataMember]
	public int Take => PageSize;
	[IgnoreDataMember]
	public int Skip => PageSize * (PageNumber - 1);
}