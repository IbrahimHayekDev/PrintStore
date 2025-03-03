using Microsoft.EntityFrameworkCore;

using PrintStoreApi.Core.Interfaces.Repositories;
using PrintStoreApi.Core.Interfaces.Services;
using PrintStoreApi.Data;
using PrintStoreApi.Models.Api;
using PrintStoreApi.Models.Common;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace PrintStoreApi.Services.Auth;

public class EmailService : IEmailService
{
	private readonly IUserRepository _userRepository;
	private readonly string _apiKey;
	private readonly string _fromEmail;
	private readonly string _fromName;
	private readonly AppDbContext _context;

	public EmailService(IConfiguration configuration, IUserRepository userRepository, AppDbContext context)
	{
		_userRepository = userRepository;
		_fromName = configuration["SendGrid:FromName"];
		_fromEmail = configuration["SendGrid:FromEmail"];
		_apiKey = configuration["SendGrid:ApiKey"];
		_context = context;

	}

	public async Task<bool> SendVerificationEmailAsync(string toEmail, string VerificationLink)
	{
		var client = new SendGridClient(_apiKey);
		var from = new EmailAddress(_fromEmail, _fromName);
		var subject = "Verify Your Email";
		var to = new EmailAddress(toEmail);
		var htmlContent = $"<p>Click <a href='{VerificationLink}'>Here</a> to verify your email.</p>";
		var msg = MailHelper.CreateSingleEmail(from, to,subject,null,htmlContent);
		var response = await client.SendEmailAsync(msg);
		if (!response.IsSuccessStatusCode)
		{
			//throw new Exception($"Email sending failed:{response.StatusCode}");
			return false;
		}
		return true;

	}

	public async Task<Response<string>> VerifyEmail(ValidateAccountRequest request)
	{
		var response = new Response<string>();
		var user =  await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.Email);

		if (user == null) {
			response.Error.Errors.Add("Invalid User");
			return response;
		}
		if (user.EmailConfirmed == 1)
		{
			response.apiMessage = new ApiMessage
			{
				message = "The account is already verified.",
				showMessage = true,
			};
			return response;

		}
		if (user.EmailVerificationToken != request.Token)
		{
			response.Error.Errors.Add("Invalid Token");
			return response;
		}
		user.EmailConfirmed = 1;
		user.EmailVerificationToken = "";
		await _userRepository.editUser(user);
		response.Data = "User Verified";
		response.apiMessage = new ApiMessage
		{
			message = "The account has been verified successfully.",
			showMessage = true,
		};
		return response;
	}

}
