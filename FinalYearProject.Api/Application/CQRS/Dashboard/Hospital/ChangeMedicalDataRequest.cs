using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FinalYearProject.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace FinalYearProject.Api.Application.CQRS.Dashboard.Hospital;

public class ChangeMedicalDataRequest : IRequest<BaseResponse>
{
    internal long UserId { get; set; }
    public string PrivateKey { get; set; } = default!;
    public string PrivateExponentKey { get; set; } = default!;
    public int RequestID { get; set; }
    
    internal bool IsApproved { get; set; } 
}

public class AcceptMedicalDataRequestValidator : AbstractValidator<ChangeMedicalDataRequest>
{
    public AcceptMedicalDataRequestValidator()
    {
        RuleFor(x => x.PrivateKey).NotNull().NotEmpty().When(x => x.IsApproved);
        RuleFor(x => x.PrivateExponentKey).NotNull().NotEmpty().When(x => x.IsApproved);
        RuleFor(x => x.RequestID).NotNull().NotEmpty();
    }
}

public class AcceptMedicalDataRequestHandler : IRequestHandler<ChangeMedicalDataRequest, BaseResponse>
{
    private readonly FinalYearDBContext _context;
    private readonly IEncryptionService _encryption;
    private readonly IUtilityService _utility;
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;
    private readonly ILogger<AcceptMedicalDataRequestHandler> _logger;
    private readonly IHybridEncryption _hybrid;

    public AcceptMedicalDataRequestHandler(FinalYearDBContext context,
        IEncryptionService encryption,
        IUtilityService utility,
        IAccountService accountService,
        IEmailService emailService,
        ILogger<AcceptMedicalDataRequestHandler> logger,
        IHybridEncryption hybrid

        )
    {
        _context = context;
        _encryption = encryption;
        _utility = utility;
        _accountService = accountService;
        _emailService = emailService;
        _logger = logger;
        _hybrid = hybrid;
    }

    public async Task<BaseResponse> Handle(ChangeMedicalDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.UserType == UserType.Hospital, cancellationToken);
            if (user is null)
                return new BaseResponse(false, "THe User Tied to this operation was not found");

            var medicalrequest = await _context.HospitalRequests.FirstOrDefaultAsync(x => x.Id == request.RequestID && x.HospitalId == user.Id ,cancellationToken);
            if (medicalrequest is null)
                return new BaseResponse(false, "The request tied to this operation was not found");
            var research = await _context.Users.AsNoTracking().Select(x => new { x.FirstName, x.Id,x.Email }).FirstOrDefaultAsync(x => x.Id == medicalrequest.ResearchCenterId, cancellationToken);
            medicalrequest.IsApproved = request.IsApproved;
            medicalrequest.TimeUpdated = DateTimeOffset.UtcNow;
            medicalrequest.status = request.IsApproved ? DataRequestSatusEnum.ACCEPTED : DataRequestSatusEnum.REJECTED;
            await _context.SaveChangesAsync(cancellationToken);
            if (medicalrequest.IsApproved)
            {
                //send email for approvalincluding files
                var record = await _context.MedicalDataRecords.AsNoTracking().FirstOrDefaultAsync(x => x.Id == medicalrequest.MedicalRecordID, cancellationToken);
                var sdtm = record!.SDTMRecordBytes;
                var icd = record!.ICDRecordBytes;
                var userrsa = await _context.UserRSA.FirstOrDefaultAsync(x => x.UserID == user.Id, cancellationToken);
                if(userrsa is null)
                {
                    return new BaseResponse(false, "Nor RSa Details Found");
                }
                var privrsa = JsonConvert.DeserializeObject<RSAParameters>(userrsa.PrivateRSAParameters);
                var privateKey = new RSAParameters
                {
                    Modulus = Convert.FromBase64String(request.PrivateKey),
                    Exponent = Convert.FromBase64String(request.PrivateExponentKey),
                    // ... other necessary RSAParameters fields for the private key
                    D = privrsa.D,
                    P = privrsa.P,
                    Q = privrsa.Q,
                    DP =privrsa.DP,
                    DQ = privrsa.DQ,
                    InverseQ =  privrsa.InverseQ
                };
                var sdtms = _hybrid.DecryptData(sdtm, privrsa).Data;
                var icds = _hybrid.DecryptData(icd, privrsa).Data;
                var sdtmrecord = JsonConvert.DeserializeObject<List<SDTMDataset>>(sdtms!);
                var icdrecord = JsonConvert.DeserializeObject<List<ICDDataset>>(icds!);
                string sdtmCsv = _utility.ConvertToCsv(sdtmrecord!);
                string icdCsv = _utility.ConvertToCsv(icdrecord!);

                //Convert CSV strings to Base64
                string sdtmBase64 = _utility.ConvertToBase64(sdtmCsv);
                string icdBase64 = _utility.ConvertToBase64(icdCsv);

                List<string>? AttachementBase64String = new();
                List<string>? AttachementName = new();
                List<string>? AttachementType = new();
                AttachementBase64String.Add(sdtmBase64);
                AttachementBase64String.Add(icdBase64);
                AttachementName.Add("sdtm.csv");
                AttachementName.Add("icd.csv");
                AttachementType.Add("text/csv");
                AttachementType.Add("text/csv");

                var sendemail = await _accountService.SendMedicalRecordsAsync(new SendMedicalDataRequest
                {
                    HOSPITALNAME = user.FirstName,
                    RESEARCHNAME = research!.FirstName,
                    AttachementBase64String = AttachementBase64String,
                    AttachementName = AttachementName,
                    AttachementType = AttachementType,
                    Email = research.Email,
                    UserId = user.Id
                }, cancellationToken);


            }
            else
            {
                //send rejection email

                var emailBodyRequest = new EmailBodyRequest { Email = research!.Email, FirstName = research.FirstName, EmailTitle = EmailTitleEnum.DOCUMENTREJECTED };
                var emailBodyResponse = await _emailService.GetEmailBody(emailBodyRequest);
                var emailBody = emailBodyResponse.Data;
                if (!emailBodyResponse.Status)
                    return new BaseResponse(false, emailBodyResponse.Message!);
                string? htmlEmailBody = emailBody!.HtmlBody;
                string? plainEmailBody = emailBody.PlainBody;
                htmlEmailBody?.Replace("[[NAME]]", research.FirstName);
                htmlEmailBody?.Replace("[[HOSPITAL]]", user.FirstName);

                plainEmailBody?.Replace("[[NAME]]", research.FirstName);
                plainEmailBody?.Replace("[[HOSPITAL]]", user.FirstName);

                await _emailService.SendEmailAsync(new SingleEmailRequest
                {
                    RecipientEmailAddress = research.Email,
                    RecipientName = research.FirstName,
                    EmailSubject = $"Medical Data Rejected",
                    HtmlEmailBody = htmlEmailBody,
                    PlainEmailBody = plainEmailBody,
                });
            }

            return new BaseResponse(true, medicalrequest.IsApproved ? "Request Approved Successfully" : "Request Rejected Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An Error Occured while trying to Approve Document");
            return new BaseResponse(false, $"An Error Occured While trying to Change the Document Status");
        }
        
    }
}
