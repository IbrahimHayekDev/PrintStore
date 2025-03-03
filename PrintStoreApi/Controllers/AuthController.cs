using Azure.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using PrintStoreApi.Core.Interfaces.Repositories;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Models.Api;
using PrintStoreApi.Repositories;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrintStoreApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly IEmailService _emailService;
	private readonly IAuthRepository _userRepository;

	public AuthController(IAuthService authService, IAuthRepository userRepository, IEmailService emailService)
	{
		_authService = authService;
		_userRepository = userRepository;
		_emailService = emailService;
	}

	[HttpPost("signup")]
	[AllowAnonymous]
	public async Task<IActionResult> Signup([FromBody] RegisterRequestDTO request)
	{
		var result = await _authService.RegisterAsync(request);
		if (result == null)
			return BadRequest(new { message = result });

		return Ok(result);
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
	{
		var token = await _authService.LoginAsync(request);
		if (token == null)
			return Unauthorized(new { message = "Invalid credentials" });

		return Ok(token);
	}

	[HttpPost("logout")]
	public async Task<IActionResult> Logout([FromHeader(Name ="Authorization")] string authorizationHeader)
	{
		if(string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer"))
			return BadRequest(new {message = "Invalid Token"});
		var token = authorizationHeader.Substring("Bearer".Length).Trim();

		//Decode JWT to get expiry time

		var handler = new JwtSecurityTokenHandler();
		var jwtToken = handler.ReadJwtToken(token);
		var expiryDate = jwtToken.ValidTo;
		await _userRepository.RevokeTokenAsync(token, expiryDate);
		return Ok(new {message = "Logged out successfully"});
	}

	[HttpPost("validateAccount")]
	[AllowAnonymous]
	public async Task<IActionResult> ValidateAccount([FromBody] ValidateAccountRequest request)
	{
		var result = await _emailService.VerifyEmail(request);
		if (result == null)
			return BadRequest(new { message = result });

		return Ok(result);
	}
}
