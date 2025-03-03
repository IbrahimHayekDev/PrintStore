using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PrintStoreApi.Core.Entities.User;

public class UserDB
{
	[Key]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(50)]
	public string FirstName { get; set; }

	[Required]
	[MaxLength(50)]
	public string LastName { get; set; }

	[MaxLength(50)]
	public string MobileNumber { get; set; }

	[Required]
	[EmailAddress]
	[MaxLength(100)]
	public string Email { get; set; }

	[Required]
	public string PasswordHash { get; set; }

	[Required]
	public int RoleId { get; set; }

	[ForeignKey("RoleId")]
	public Role Role { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public int EmailConfirmed { get; set; }
	[AllowNull]
	public string? EmailVerificationToken { get; set; }
}
