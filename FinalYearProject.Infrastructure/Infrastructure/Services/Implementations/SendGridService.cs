using System.Net.Mail;
using System.Net;
using System.Text;
using FinalYearProject.Infrastructure.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.DataExtension;
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Services.Interfaces;

namespace Payultra.Infrastructure.Services.Implementations
{
    public class SendGridService : ISendGridService
    {
        private readonly IConfiguration _config;
        private readonly FinalYearDBContext _context;
        private readonly ILogger<SendGridService> _logger;

        public SendGridService(
            IConfiguration config,
            FinalYearDBContext context,
            ILogger<SendGridService> logger
        )
        {
            _config = config;
            _context = context;
            _logger = logger;
        }

        public async Task<BaseResponse> SendEmailAsync(SingleEmailRequest request)
        {
            _logger?.LogInformation($"SENDGRID_SEND_EMAIL => Sending email attempt with sendgrid | Email - {request.RecipientEmailAddress}");
            try
            {
                var client = new SendGridClient(_config["SendGrid:APIKey"]);
                var from = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromName"]);
                string subject = request.EmailSubject ?? "";
                var to = new EmailAddress(request.RecipientEmailAddress, request.RecipientName);
                var plainTextContent = request.PlainEmailBody;
                var htmlContent = request.HtmlEmailBody;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                if (!string.IsNullOrWhiteSpace(request.AttachementBase64String) && !string.IsNullOrWhiteSpace(request.AttachementName))
                {
                    //msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment> { new SendGrid.Helpers.Mail.Attachment { Filename = request.AttachementName, Content = request.AttachementBase64String, Type = request.AttachementType, Disposition = "attachment" } };
                    msg.AddAttachment(request.AttachementName, request.AttachementBase64String, request.AttachementType, "attachment");
                }

                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

                if (response?.StatusCode == HttpStatusCode.Accepted) return new BaseResponse(true, "Email send successfully");
                string errorMessage = "";
                if (response != null)
                {
                    string responseMessage = await response.Body.ReadAsStringAsync().ConfigureAwait(false);
                    var responseObject = JsonConvert.DeserializeObject<SendGridErrorResponse>(responseMessage);
                    var errors = responseObject?.Errors?.Select(x => x.Message).ToList();
                    errorMessage = string.Join(" ", errors ?? new List<string?>());
                   
                }

                _logger?.LogInformation($"SENDGRID_SEND_EMAIL => Sending email attempt was not successful. | Reason - {errorMessage}");
                return new BaseResponse(false, errorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "SENDGRID_SEND_EMAIL => Application ran into an error while trying to send email");
                return new BaseResponse(false, "Sending email attempt was not successful.");
            }
        }

        public async Task<BaseResponse> SendEmailToMultipleRecipientsAsync(MultipleEmailRequest request)
        {
            _logger?.LogInformation($"SENDGRID_SEND_EMAIL => Sending email attempt with sendgrid | Email - {string.Join(",", request.RecipientEmailAddresses!)}");
            try
            {
                string errorMessage = "";

                if (request.RecipientEmailAddresses.IsAny())
                {
                    var emailsAddressess = new List<EmailAddress>();
                    foreach (var emailAddress in request.RecipientEmailAddresses!)
                    {
                        emailsAddressess.Add(new EmailAddress { Email = emailAddress, Name = emailAddress });
                    }

                    var client = new SendGridClient(_config["SendGrid:APIKey"]);
                    var from = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromName"]);
                    string subject = request.EmailSubject ?? "";
                    var to = emailsAddressess;
                    var plainTextContent = request.PlainEmailBody;
                    var htmlContent = request.HtmlEmailBody;
                    var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);

                    if (!string.IsNullOrWhiteSpace(request.AttachementBase64String) && !string.IsNullOrWhiteSpace(request.AttachementName))
                    {
                        //msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment> { new SendGrid.Helpers.Mail.Attachment { Filename = request.AttachementName, Content = request.AttachementBase64String, Type = request.AttachementType, Disposition = "attachment" } };
                        msg.AddAttachment(request.AttachementName, request.AttachementBase64String, request.AttachementType, "attachment");
                    }

                    var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

                    if (response?.StatusCode == HttpStatusCode.Accepted) return new BaseResponse(true, "Email send successfully");
                    if (response != null)
                    {
                        string responseMessage = await response.Body.ReadAsStringAsync().ConfigureAwait(false);
                        var responseObject = JsonConvert.DeserializeObject<SendGridErrorResponse>(responseMessage);
                        var errors = responseObject?.Errors?.Select(x => x.Message).ToList();
                        errorMessage = string.Join(" ", errors ?? new List<string?>());
                    }
                }

                _logger?.LogInformation($"SENDGRID_SEND_EMAIL => Sending email attempt was not successful. | Reason - {errorMessage}");
                return new BaseResponse(false, errorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "SENDGRID_SEND_EMAIL => Application ran into an error while trying to send email");
                return new BaseResponse(false, "Sending email attempt was not successful.");
            }
        }
        //}

        public async Task<BaseResponse> SendEmailWithSMTP(SingleEmailRequest request)
        {
            _logger?.LogInformation($"SMTP_SEND_MAIL_SERVICE => Sending email attempt with sendgrid | Email - {request.RecipientEmailAddress}");
            try
            {
                MailMessage mail = new MailMessage
                {
                    Subject = request.EmailSubject,
                    Body = request.HtmlEmailBody,
                    From = new MailAddress(_config["SMTPConfig:Sender"]!, _config["SMTPConfig:DisplayName"]),
                    To = { request.RecipientEmailAddress! },
                    IsBodyHtml = bool.Parse(_config["SMTPConfig:IsBodyHtml"]!)
                };

                NetworkCredential networkCredential = new NetworkCredential(_config["SMTPConfig:Username"], _config["SMTPConfig:Password"]);

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _config["SMTPConfig:Host"]!,
                    Port = int.Parse(_config["SMTPConfig:Port"]!),
                    EnableSsl = bool.Parse(_config["SMTPConfig:EnableSSL"]!),
                    UseDefaultCredentials = bool.Parse(_config["SMTPConfig:UseDefaultCredentials"]!),
                    Credentials = networkCredential
                };

                mail.BodyEncoding = Encoding.Default;
                await smtpClient.SendMailAsync(mail).ConfigureAwait(false);

                return new BaseResponse(true, "Email send successful");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "SMTP_SEND_MAIL_SERVICE => Application ran into an error while trying to send email");
                return new BaseResponse(false, "Application ran into an error while trying to send email");
            }
        }
    }
}