using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FinalYearProject.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Registration;

public class RegisterHospitalRequest : IRequest<BaseResponse>
{
#nullable disable
    public string? Location { get; set; }
    public string? HospitalName { get; set; }
    public string? HospitalEmail { get; set; }
    public string? WebsiteAddress { get; set; }
    public IFormFile? CacDocument { get; set; }
    public IFormFile? NafdacDocument { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public UserType AccountType { get; set; }
}

public class RegisterHospitalRequestValidator : AbstractValidator<RegisterHospitalRequest>
{
    public RegisterHospitalRequestValidator()
    {
        RuleFor(x => x.Location)
         .NotEmpty()
         .MaximumLength(100);

        RuleFor(x => x.HospitalName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.HospitalEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.WebsiteAddress).NotNull().NotEmpty();
        RuleFor(x => x.WebsiteAddress)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.WebsiteAddress)).WithMessage("Website Address Must Contain 'https' example 'https://mydomain.com'");

       RuleFor(x => x.CacDocument).NotNull().NotEmpty();
        RuleFor(x => x.NafdacDocument).NotNull().NotEmpty();

        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password field is required.")
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!#%*?&])[A-Za-z\d@$!#%*?&]{8,}$").WithMessage("Invalid password format");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");

        RuleFor(x => x.AccountType)
            .IsInEnum();
    }

    private bool BeAValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

}


public class RegisterHospitalRequestHandler : IRequestHandler<RegisterHospitalRequest, BaseResponse>
{
    private readonly FinalYearDBContext _context;
    private readonly ILogger<RegisterHospitalRequestHandler> _logger;
    private ICloudinaryService _cloudinary;
    private UserManager<DataAggregatorUser> _usermanager;
    private readonly IAccountService _account;
    public RegisterHospitalRequestHandler(FinalYearDBContext context,
        ILogger<RegisterHospitalRequestHandler> logger,
        ICloudinaryService cloudinary,
        UserManager<DataAggregatorUser> usermanager,
        IAccountService account
        )
    {
        _context = context;
        _logger = logger;
        _cloudinary = cloudinary;
        _usermanager = usermanager;
        _account = account;
    }

    public async Task<BaseResponse> Handle(RegisterHospitalRequest request, CancellationToken cancellationToken)
    {
         
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try {
                var existinguser = await _context.Users.AsNoTracking().Where(x => x.Email!.ToLower()! == request.HospitalEmail!.ToLower()).FirstOrDefaultAsync();
                if (existinguser is not null)
                {
                    _logger.LogInformation($"REGISTER_HOSPITAL_REQUEST =>  User with email {request.HospitalEmail} was taken");
                    return new BaseResponse(false, "This Email Already Exisits");
                }
                var cacstream = new MemoryStream();

                using (var stream = request.CacDocument!.OpenReadStream())
                {
                    await stream.CopyToAsync(cacstream);
                }
                cacstream.Position = 0;

                var cacupload = await _cloudinary.UploadImageAsync(cacstream, Guid.NewGuid().ToString());
                var nafstream = new MemoryStream();

                using (var stream = request.NafdacDocument!.OpenReadStream())
                {
                    await stream.CopyToAsync(nafstream);
                }
                nafstream.Position = 0;
                var nafdacupload = await _cloudinary.UploadImageAsync(nafstream,Guid.NewGuid().ToString());

                if (!cacupload.Status || !nafdacupload.Status)
                {
                    return new BaseResponse(false, "An Error Occured WHile Processing Documents Please Contact Support After Many Tries");
                }
                var cacInfo = new Document
                {
                    Path = cacupload.Message,
                    Title = request.CacDocument.FileName,

                };               
                
                var nafdacInfo = new Document
                {
                    Path = nafdacupload.Message,
                    Title = request.NafdacDocument.FileName,

                };
                await _context.AddRangeAsync(cacInfo, nafdacInfo);
                await _context.SaveChangesAsync(cancellationToken);
                var hospinfo = new HospitalInfo
                {
                    CacDocumentID = cacInfo.Id,
                    HospitalName = request.HospitalName,
                    HospitalWebsite = request.WebsiteAddress,
                    NafdacDocumentID = nafdacInfo.Id,
                };

                 await _context.AddAsync(hospinfo);
                await _context.SaveChangesAsync(cancellationToken);

                var user = new DataAggregatorUser()
                {
                    Email = request.HospitalEmail!.ToLower(),
                    HospitalInfo = hospinfo.ID,
                    FirstName = hospinfo.HospitalName,
                    AccountStatus = AccountStatusEnum.InActive,
                    UserType = UserType.Hospital,
                    TimeCreated = DateTime.UtcNow,
                    TimeUpdated = DateTime.UtcNow,
                    NormalizedEmail = request.HospitalEmail!.ToUpperInvariant(),
                    NormalizedUserName = request.HospitalEmail!.ToUpperInvariant(),
                    UserName = request.HospitalEmail.ToLower()!

                };
               
                await _context.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var sendemail = await _account.SendOTPAsync(new SendOTPRequest
                {
                    FirstName = request.HospitalName!,
                    OtpCodeLength = OtpCodeLengthEnum.Six,
                    Purpose = OtpVerificationPurposeEnum.EmailConfirmation,
                    Recipient = request.HospitalEmail,
                    RecipientType = OtpRecipientTypeEnum.Email,
                    UserId = user.Id

                },cancellationToken);
                if (!sendemail.Status)
                {
                    return new BaseResponse(false, sendemail.Message!);
                }
                var addpassword = await _usermanager.AddPasswordAsync(user, request.Password);
                if (!addpassword.Succeeded)
                    return new BaseResponse(false, "An Error Occured While Trying to Complete your Registration");
                var requesthospital = new Request { 
                    DateRequested = DateTimeOffset.UtcNow,
                    IsApproved = false,
                    Documents = $";{cacInfo.Id};{nafdacInfo.Id};",
                    UserID = user.Id,                
                };
                await _context.AddAsync(requesthospital, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                hospinfo.UserId = user.Id;
                cacInfo.UserID = user.Id;
                nafdacInfo.UserID = user.Id;

                 _context.UpdateRange(hospinfo, cacInfo,nafdacInfo);
                await _context.SaveChangesAsync(cancellationToken);
               await transaction.CommitAsync(cancellationToken);
                return new BaseResponse(true, "Registration Successfull");
            
            }catch (Exception ex)
            {
                _logger.LogError(ex,$"REGISTER_HOSPITAL_REQUEST =>something went wrong ");
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                return new BaseResponse(false, "An Error Occured While Trying To Complete Your Registration"); ;
            }
            

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"REGISTER_HOSPITAL_REQUEST =>something went wrong ");
            return new BaseResponse(false, "An Error Occured While Trying To COmplete Your Registration"); ;
        }
    }
}