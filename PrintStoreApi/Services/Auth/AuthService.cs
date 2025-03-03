using PrintStoreApi.Core.Interfaces.Repositories;
using PrintStoreApi.Core.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PrintStoreApi.Models.Common;
using PrintStoreApi.Models.Api;
using PrintStoreApi.Core.Entities.User;
using PrintStoreApi.Models.User;

namespace PrintStoreApi.Services.Auth;

public class AuthService: IAuthService
{
	private readonly IAuthRepository _authRepository;
	private readonly IConfiguration _configuration;
	private readonly IEmailService _emailService;
	public AuthService(IAuthRepository authRepository, IConfiguration configuration, IEmailService emailService)
	{
		_authRepository = authRepository;
		_configuration = configuration;
		_emailService = emailService;
	}

	public async Task<Response<SignupResponseDTO>> RegisterAsync(RegisterRequestDTO request)
	{
		var response = new Response<SignupResponseDTO>();

		var existingUser = await _authRepository.GetByEmailAsync(request.Email);
		if (existingUser != null)
			{
			response.Error.Errors.Add("User already exists.");
			return response;
		} 

		var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
		var user = new UserDB
		{
			FirstName = request.FirstName,
			LastName = request.LastName,
			MobileNumber = request.MobileNumber,
			Email = request.Email,
			PasswordHash = passwordHash,
			RoleId = 1,
		};
		user.EmailVerificationToken = Guid.NewGuid().ToString();

		var verificationLink = $"{_configuration["FrontApp:VerifyEmailLink"]}/{user.EmailVerificationToken}/{user.Email}";
		var EmailSent = await _emailService.SendVerificationEmailAsync(user.Email, verificationLink);
		if (!EmailSent)
		{
			response.Error.Errors.Add("An error occured while sending the verification email.");
			return response;
		}
		await _authRepository.AddAsync(user);
		response.Data = new SignupResponseDTO
		{
			email= request.Email,
		};
		response.apiMessage = new ApiMessage
		{
			showMessage = true,
			message = $"User registered successfully and a verification email has been sent to '{request.Email}'."
		};
		return response;
	}

	public async Task<Response<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
	{
		var response = new Response<LoginResponseDTO>();

		var user = await _authRepository.GetByEmailAsync(request.Email);
		if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) { 
			response.Error.Errors.Add("Invalid credentials");
			return response;
		}
		if(user.EmailConfirmed == 0)
		{
			response.Error.Errors.Add("User is not verified. Please check your email to verify your account.");
			response.Error.ErrorCode = 1;
			return response;
		}
		var token = GenerateJwtToken(user);
		response.Data = new LoginResponseDTO
		{
			token= token,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			MobileNumber = user.MobileNumber,
			Role = new RoleDTO
			{
				Id = user.Role.Id,
				Name = user.Role.Name,
			},
			EmailConfirmed= user.EmailConfirmed == 0 ? false : true
		};
		response.apiMessage = new ApiMessage
		{
			showMessage = true,
			message = "Login successfully."
		};
		return response;
	}

	private string GenerateJwtToken(UserDB user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Email)
				}),
			Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtConfig:ExpiryInMinutes"])),
			Issuer = _configuration["JwtConfig:Issuer"],
			Audience = _configuration["JwtConfig:Audience"],
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

}
