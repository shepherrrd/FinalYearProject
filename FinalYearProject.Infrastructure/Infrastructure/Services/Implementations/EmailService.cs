using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FinalYearProject.Infrastructure.Data.Entities;

namespace FinalYearProject.Infrastructure.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ISendGridService _sendGridService;
        private readonly ILogger<EmailService> _logger;
        private readonly FinalYearDBContext _context;

        public EmailService(
             ILogger<EmailService> logger,
            FinalYearDBContext context,
            IWebHostEnvironment hostEnvironment,
            IHttpClientFactory clientFactory,
            ISendGridService sendGridService
            )
        {
            _logger = logger;
            _context = context;
            _hostEnvironment = hostEnvironment;
            _clientFactory = clientFactory;
            _sendGridService = sendGridService;
        }

        public async Task<BaseResponse> CheckDisposableEmailAsync(string email)
        {
            _logger.LogInformation($"EMAIL_SERVICE => Checking disposable email | Email - {email}");

            if (_hostEnvironment.EnvironmentName == "Development")
            {
                _logger.LogInformation($"EMAIL_SERVICE => Check cancelled. Environment is development | Email - {email}");
                return new BaseResponse(true, "Operation finished. Environment is development.");
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://verifier.meetchopra.com/verify/{email}?token=2b1e810090b21cab8a8753ec6bd1f091ebd1d9dd6b6ddbfea9e9b45c1b55757fba354cbe95a14fcad448107ae138e450");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"EMAIL_SERVICE => Checking disposable email completed | Result - {responseString}");

                    var responseResult = JsonConvert.DeserializeObject<MeetchopraValidResponse>(responseString);
                    return new BaseResponse(responseResult?.status ?? false, "Operation completed.");
                }
                else
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"EMAIL_SERVICE => Checking disposable email completed with error | Result - {responseString}");
                    return new BaseResponse(false, "Operation failed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"EMAIL_SERVICE => Checking disposable email completed with exception");
                return new BaseResponse(true, "Operation finished with exception.");
            }
        }

        public async Task<BaseResponse> SendEmailAsync(SingleEmailRequest request)
        {
            _logger?.LogInformation($"EMAIL_SERVICE => Sending email to {request.RecipientEmailAddress}");
            var response = await _sendGridService.SendEmailAsync(new SingleEmailRequest
            {
                RecipientEmailAddress = request.RecipientEmailAddress,
                RecipientName = request.RecipientName,
                EmailSubject = request.EmailSubject,
                HtmlEmailBody = request.HtmlEmailBody,
                PlainEmailBody = request.PlainEmailBody,
                AttachementBase64String = request.AttachementBase64String,
                AttachementName = request.AttachementName,
                AttachementType = request.AttachementType
            });

            return new BaseResponse(response.Status, response.Message!);
        }

        public async Task<BaseResponse> SendMultipleEmailAsync(MultipleEmailRequest request)
        {
            _logger?.LogInformation($"EMAIL_SERVICE => Sending email to {string.Join(",", request.RecipientEmailAddresses!)}");
            var response = await _sendGridService.SendEmailToMultipleRecipientsAsync(new MultipleEmailRequest
            {
                RecipientEmailAddresses = request.RecipientEmailAddresses,
                RecipientName = request.RecipientName,
                EmailSubject = request.EmailSubject,
                HtmlEmailBody = request.HtmlEmailBody,
                PlainEmailBody = request.PlainEmailBody,
                AttachementBase64String = request.AttachementBase64String,
                AttachementName = request.AttachementName,
                AttachementType = request.AttachementType
            });

            return new BaseResponse(response.Status, response.Message!);
        }

        public async Task<BaseResponse<EmailBodyResponse>> GetEmailBody(EmailBodyRequest request)
        {
            string emailFolderName = "emails"; string plaintTextFolderName = "plaintexts";
            var response = new EmailBodyResponse { HtmlBody = "", PlainBody = "" };
            string fileName = "", plainFilename = "";

            string? templateFile = null; string? plainTextFile = null;

            switch (request.EmailTitle)
            {
                case EmailTitleEnum.EMAILVERIFICATION:
                    fileName = "verifyemail.html";
                    templateFile = await GetTemplateFileAsync(emailFolderName, fileName);
                    if (!string.IsNullOrWhiteSpace(templateFile))
                        response.HtmlBody = templateFile;

                    plainFilename = "verifyemail.txt";
                    plainTextFile = await GetTemplateFileAsync(plaintTextFolderName, plainFilename);
                    if (!string.IsNullOrWhiteSpace(plainTextFile))
                        response.PlainBody = plainTextFile;
                    break;

                case EmailTitleEnum.PASSWORDRESET:
                    fileName = "passwordreset.html";
                    templateFile = await GetTemplateFileAsync(emailFolderName, fileName);
                    if (!string.IsNullOrWhiteSpace(templateFile))
                        response.HtmlBody = templateFile;

                    plainFilename = "passwordreset.txt";
                    plainTextFile = await GetTemplateFileAsync(plaintTextFolderName, plainFilename);
                    if (!string.IsNullOrWhiteSpace(plainTextFile))
                        response.PlainBody = plainTextFile;
                    break;

                case EmailTitleEnum.WELCOME:
                    fileName = "welcomehospitaluser.html";
                    templateFile = await GetTemplateFileAsync(emailFolderName, fileName);
                    if (!string.IsNullOrWhiteSpace(templateFile))
                        response.HtmlBody = templateFile;

                    plainFilename = "welcomehospitaluser.txt";
                    plainTextFile = await GetTemplateFileAsync(plaintTextFolderName, plainFilename);
                    if (!string.IsNullOrWhiteSpace(plainTextFile))
                        response.PlainBody = plainTextFile;
                    break;
                case EmailTitleEnum.DOCUMENTAPPROVED:
                    fileName = "medicalrecordsapproved.html";
                    templateFile = await GetTemplateFileAsync(emailFolderName, fileName);
                    if (!string.IsNullOrWhiteSpace(templateFile))
                        response.HtmlBody = templateFile;

                    plainFilename = "medicalrecordsapproved.txt";
                    plainTextFile = await GetTemplateFileAsync(plaintTextFolderName, plainFilename);
                    if (!string.IsNullOrWhiteSpace(plainTextFile))
                        response.PlainBody = plainTextFile;
                    break;



                case EmailTitleEnum.CREATENEWUSER:
                    fileName = "welcomenewuser.html";
                    templateFile = await GetTemplateFileAsync(emailFolderName, fileName);
                    if (!string.IsNullOrWhiteSpace(templateFile))
                        response.HtmlBody = templateFile;

                    plainFilename = "welcomenewuser.txt";
                    plainTextFile = await GetTemplateFileAsync(plaintTextFolderName, plainFilename);
                    if (!string.IsNullOrWhiteSpace(plainTextFile))
                        response.PlainBody = plainTextFile;
                    break;

            }

            if (string.IsNullOrEmpty(response.HtmlBody) || string.IsNullOrEmpty(response.PlainBody))
                return new BaseResponse<EmailBodyResponse>(false, "Could not fetched email body, application ran into an error while fetching email body.");

            return new BaseResponse<EmailBodyResponse>(true, "Email body fetched successfully.", response);
        }


        public async Task<string> GetTemplateFileAsync(string folderName, string fileName)
        {
            string templateFile = string.Empty;
            try
            {
                string path = Path.Combine(_hostEnvironment.WebRootPath, "Assets", folderName);
                string filePath = Path.Combine(path, fileName);
                templateFile = await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EmailUtil => Application ran into an error while fetching email template file");
            }

            if (templateFile != null)
                _logger.LogInformation("EmailUtil => Email template fetched");

            return templateFile!;
        }
    }
}
