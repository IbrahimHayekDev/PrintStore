using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintStoreApi.Core.Entities.User;

public class Role
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(50)]
	public string Name { get; set; }

}
