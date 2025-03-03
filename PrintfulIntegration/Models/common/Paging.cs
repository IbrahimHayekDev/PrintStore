using System.Runtime.Serialization;

namespace PrintfulIntegration.Models.common;

public class Paging
{
	public int Total { get; set; } = 1;
	public int Offset { get; set; } = 10;
	[IgnoreDataMember]
	public int Limit { get; set; }
	//[IgnoreDataMember]
	//public int Skip => PageSize * (PageNumber - 1);
}
