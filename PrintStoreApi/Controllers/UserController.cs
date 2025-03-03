using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PrintStoreApi.Core.Interfaces.Reprositories;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.User;
using PrintStoreApi.Services.Auth;

using System.Security.Claims;

namespace PrintStoreApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
 	}

	[HttpGet("getUserData")]
	[Authorize]
	public async Task<IActionResult> GetUserById()
	{
		var userid = (User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "O").ToString();
		var user = await _userService.GetUserById(userid);
		if (user == null)
			return BadRequest(new { message = user });

		return Ok(user);
	}

	[HttpPost("editUser")]
	[Authorize]
	public async Task<IActionResult> EditUserData(EditUserRequest request)
	{
		var userid = (User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "O").ToString();
		var userUpdated = await _userService.EditUserAsync(userid, request);
		if (userUpdated == null)
			return BadRequest(new { message = userUpdated });

		return Ok(userUpdated);
	}

}
