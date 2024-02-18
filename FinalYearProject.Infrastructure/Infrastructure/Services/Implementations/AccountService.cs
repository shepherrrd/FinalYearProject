using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FinalYearProject.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace FinalYearProject.Infrastructure.Infrastructure.Services.Implementations;


    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly FinalYearDBContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IUtilityService _utilityService;
        private readonly IServiceProvider _serviceProvider;

        public AccountService(
             ILogger<AccountService> logger,
            FinalYearDBContext context,
            IConfiguration config,
            IEmailService emailService,
            IUtilityService utilityService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _context = context;
            _config = config;
            _emailService = emailService;
            _utilityService = utilityService;
            _serviceProvider = serviceProvider;
        }

        public async Task<BaseResponse<OtpRequestResult>> SendOTPAsync(SendOTPRequest request, CancellationToken cancellationToken)
        {
            var code = request.OtpCodeLength == OtpCodeLengthEnum.Four ? _utilityService.GenerateRandomNumber(4) : _utilityService.GenerateRandomNumber(6);
            _logger?.LogInformation($"EmailService > Sending OTP to {request.Recipient}");

            /* Invalidate existing sent OTPs */
            await _context.OtpVerifications.Where(x => x.UserId.Equals(request.UserId) && x.Purpose == request.Purpose && x.Status == OtpCodeStatusEnum.Sent)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(column => column.Status, OtpCodeStatusEnum.Invalidated)
                .SetProperty(column => column.TimeUpdated, DateTimeOffset.UtcNow), cancellationToken);

            /* Persist new OTP */
            var otpVerification = new OtpVerification
            {
                UserId = request.UserId,
                Code = code,
                Recipient = request.Recipient,
                RecipientType = request.RecipientType,
                TimeCreated = DateTimeOffset.UtcNow,
                TimeUpdated = DateTimeOffset.UtcNow,
                Status = OtpCodeStatusEnum.Sent,
                Purpose = request.Purpose
            };
            await _context.AddAsync(otpVerification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            if (request.RecipientType == OtpRecipientTypeEnum.Email)
            {
                /* Generate email body */
                await SendEmailOtpAsync(request, code, "FInal Year Project" ?? "");
            }

            _logger?.LogInformation($"AccountService > OTP sent to {request.Recipient}");
            return new BaseResponse<OtpRequestResult>(true, "OTP sent successfully", new OtpRequestResult
            {
                Recipient = request.Recipient
            });
        }

        private async Task SendEmailOtpAsync(SendOTPRequest request, string code, string applicationName)
        {
            var emailBodyRequest = new EmailBodyRequest { Email = request.Recipient, FirstName = request.FirstName };

            var emailBodyResponse = new BaseResponse<EmailBodyResponse>();

            switch (request.Purpose)
            {
                case OtpVerificationPurposeEnum.EmailConfirmation:
                    emailBodyRequest.EmailTitle = EmailTitleEnum.EMAILVERIFICATION;
                    emailBodyResponse = await _emailService.GetEmailBody(emailBodyRequest);
                    break;
                case OtpVerificationPurposeEnum.PasswordReset:
                    emailBodyRequest.EmailTitle = EmailTitleEnum.PASSWORDRESET;
                    emailBodyResponse = await _emailService.GetEmailBody(emailBodyRequest);
                    break;

                default: break;
            }

            if (!emailBodyResponse.Status)
            {
                _logger?.LogInformation($"AccountService => Sending OTP to {request.Recipient} failed");
            }
            else
            {
                var emailBody = emailBodyResponse.Data;

                string? htmlEmailBody = emailBody!.HtmlBody;
                string? plainEmailBody = emailBody.PlainBody;
                Console.WriteLine(htmlEmailBody);
                htmlEmailBody = htmlEmailBody?.Replace("[[CODE]]", code.ToString());
                plainEmailBody = plainEmailBody?.Replace("[[CODE]]", code);
                Console.WriteLine(htmlEmailBody);

                await _emailService.SendEmailAsync(new SingleEmailRequest
                {
                    RecipientEmailAddress = request.Recipient,
                    RecipientName = request.FirstName,
                    EmailSubject = $"{request.Purpose.GetDescription()} | {applicationName}",
                    HtmlEmailBody = htmlEmailBody,
                    PlainEmailBody = plainEmailBody
                });
            }
        }

       public async Task<BaseResponse> SendHospitalWelcomeEmailAsync(SendHospitalKeysRequest request, CancellationToken cancellationToken)
    {
        var emailBodyRequest = new EmailBodyRequest { Email = request.Email, FirstName = request.FirstName,EmailTitle = EmailTitleEnum.WELCOME };
        var emailBodyResponse = await _emailService.GetEmailBody(emailBodyRequest);
        var emailBody = emailBodyResponse.Data;
        if(!emailBodyResponse.Status)
            return new BaseResponse(false,emailBodyResponse.Message!);
        string? htmlEmailBody = emailBody!.HtmlBody;
        string? plainEmailBody = emailBody.PlainBody;
        htmlEmailBody?.Replace("[[PUBKEY]]", request.PublicKey);
        htmlEmailBody?.Replace("[[PRIVKEY]]", request.PrivateKey);

        plainEmailBody?.Replace("[[PUBKEY]]", request.PublicKey);
        plainEmailBody?.Replace("[[PRIVKEY]]", request.PrivateKey);

        await _emailService.SendEmailAsync(new SingleEmailRequest
        {
            RecipientEmailAddress = request.Email,
            RecipientName = request.FirstName,
            EmailSubject = $"Keys for Documents",
            HtmlEmailBody = htmlEmailBody,
            PlainEmailBody = plainEmailBody
        });

        return new BaseResponse(true, "Sent");

    }

    public async Task<BaseResponse> SendMedicalRecordsAsync(SendMedicalDataRequest request, CancellationToken cancellationToken)
    {
        var emailBodyRequest = new EmailBodyRequest { Email = request.Email, FirstName = request.RESEARCHNAME, EmailTitle = EmailTitleEnum.DOCUMENTAPPROVED };
        var emailBodyResponse = await _emailService.GetEmailBody(emailBodyRequest);
        var emailBody = emailBodyResponse.Data;
        if (!emailBodyResponse.Status)
            return new BaseResponse(false, emailBodyResponse.Message!);
        string? htmlEmailBody = emailBody!.HtmlBody;
        string? plainEmailBody = emailBody.PlainBody;
        htmlEmailBody?.Replace("[[NAME]]", request.RESEARCHNAME);
        htmlEmailBody?.Replace("[[HOSPITAL]]", request.HOSPITALNAME);

        plainEmailBody?.Replace("[[NAME]]", request.RESEARCHNAME);
        plainEmailBody?.Replace("[[HOSPITAL]]", request.HOSPITALNAME);

        await _emailService.SendEmailAsync(new SingleEmailRequest
        {
            RecipientEmailAddress = request.Email,
            RecipientName = request.RESEARCHNAME,
            EmailSubject = $"Medical Data Approved",
            HtmlEmailBody = htmlEmailBody,
            PlainEmailBody = plainEmailBody,
            AttachementBase64String = request.AttachementBase64String,
            AttachementName = request.AttachementName,
            AttachementType = request.AttachementType
        });

        return new BaseResponse(true, "Sent");

    }

        public async Task<BaseResponse> ValidateOTPCodeAsync(ValidateOtpRequest otpRequest, CancellationToken cancellationToken)
        {
            _logger?.LogInformation($"ACCOUNT_SERVICE Validating OTP Code=> Process started");


        var otpVerification = await _context.OtpVerifications.FirstOrDefaultAsync(v => v.UserId == otpRequest.UserId && v.Status == OtpCodeStatusEnum.Sent && v.Purpose == otpRequest.Purpose && v.Code.Equals(otpRequest.Code), cancellationToken);
        if (otpVerification == null)
        {
            _logger?.LogInformation($"ACCOUNT_SERVICE Validating OTP Code=> No pending OTP found for this user");
            return new BaseResponse(false, "Verification code provided is invalid.");
        }

        //Check expiry
        if (DateTime.UtcNow > otpVerification.TimeCreated.AddMinutes(15))
        {
            _logger?.LogInformation($"ACCOUNT_SERVICE Validating OTP Code => Validation Failed. OTP validation code has expired");

            otpVerification.Status = OtpCodeStatusEnum.Expired;
            otpVerification.TimeUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse(false, "Code provided has expired.");
        }

        otpVerification.ConfirmedOn = DateTime.UtcNow;
        otpVerification.Status = OtpCodeStatusEnum.Verified;
        otpVerification.TimeUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger?.LogInformation($"ACCOUNT_SERVICE Validating OTP Code => Code authenticated successfully");
            return new BaseResponse(true, "Code verification successfully.");
        }

    }

