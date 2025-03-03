using PrintStoreApi.Models.Api;
using PrintStoreApi.Models.Common;

namespace PrintStoreApi.Core.Interfaces.Services;

public interface IEmailService
{
	Task<bool> SendVerificationEmailAsync(string toEmail, string VerificationLink);
	Task<Response<string>> VerifyEmail(ValidateAccountRequest request);
}
