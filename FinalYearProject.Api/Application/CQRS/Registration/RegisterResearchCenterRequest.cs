using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FinalYearProject.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace FinalYearProject.Api.Application.CQRS.Registration;

public class RegisterResearchCenterRequest : IRequest<BaseResponse>
{
#nullable disable
    public UserType AccountType { get; set; } // Assuming AccountType is an enum or class
    public string Location { get; set; }

    public string Title { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Institution { get; set; }
    public string Email { get; set; }
    public IFormFile PassportPhoto { get; set; } // Assuming FileData is a class or struct
    public IFormFile Degree { get; set; }        // Same assumption as above
    public IFormFile ResearchProposal { get; set; }
    public IFormFile IrbApproval { get; set; }

    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RegisterResearchCenterRequestValidator : AbstractValidator<RegisterResearchCenterRequest>
{
    public RegisterResearchCenterRequestValidator()
    {
        RuleFor(x => x.AccountType)
        .IsInEnum(); // Assuming AccountType is an enum

        RuleFor(x => x.Location).NotNull().NotEmpty().MaximumLength(100);

        RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(50);

        RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(100);

        // Optional: MiddleName can be null or empty
        RuleFor(x => x.MiddleName).NotNull().NotEmpty().MaximumLength(100);

        RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(100);

        RuleFor(x => x.Institution).NotNull().NotEmpty().MaximumLength(200);

        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();

        // File validators
        RuleFor(x => x.IrbApproval ).Must(BeValidFileType).WithMessage(" IRBApproval Files must be either JPEG or PDF.");
        RuleFor(x => x.Degree ).Must(BeValidFileType).WithMessage(" Degree must be either JPEG or PDF.");
        RuleFor(x => x.PassportPhoto ).Must(BeValidFileType).WithMessage(" Passport Files must be either JPEG or PDF.");
        RuleFor(x => x.ResearchProposal ).Must(BeValidFileType).WithMessage(" ResearchProposal Files must be either JPEG or PDF.");

        RuleFor(x => x.Password).NotNull().NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!#%*?&])[A-Za-z\d@$!#%*?&]{8,}$").
            WithMessage("Invalid password format").MinimumLength(8); // You can add more complexity requirements as needed

        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords must match.");
    }

    private bool BeValidFileType(IFormFile file)
    {
        if (file == null) return true; // Assuming files are optional. If mandatory, return false.

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".pdf" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(fileExtension);
    }
}

public class RegisterResearchCenterRequestHandler : IRequestHandler<RegisterResearchCenterRequest, BaseResponse>
{
    private readonly FinalYearDBContext _context;
    private readonly ILogger<RegisterResearchCenterRequestHandler> _logger;
    private ICloudinaryService _cloudinary;
    private UserManager<DataAggregatorUser> _usermanager;
    private readonly IAccountService _account;
    public RegisterResearchCenterRequestHandler(FinalYearDBContext context,
        ILogger<RegisterResearchCenterRequestHandler> logger,
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
    public async Task<BaseResponse> Handle(RegisterResearchCenterRequest request, CancellationToken cancellationToken)
    {
        try
        {
        using var transanction = await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var existinguser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower()!.Equals(request.Email.ToLower()));
                if (existinguser is not null)
                {
                    _logger.LogInformation($"REGISTER_HOSPITAL_REQUEST =>  User with email {request.Email} was taken");
                    return new BaseResponse(false, "This Email Already Exisits");
                }

                var passportmemoryStream = new MemoryStream();

                request.PassportPhoto.OpenReadStream().CopyTo(passportmemoryStream);
                passportmemoryStream.Position = 0;
                var passport = await _cloudinary.UploadImageAsync(passportmemoryStream, Guid.NewGuid().ToString());

                var degreememoryStream = new MemoryStream();
                request.Degree.OpenReadStream().CopyTo(degreememoryStream);
                degreememoryStream.Position = 0;
                var degree = await _cloudinary.UploadImageAsync(degreememoryStream, Guid.NewGuid().ToString());

                var researchmemoryStream = new MemoryStream();
                request.ResearchProposal.OpenReadStream().CopyTo(researchmemoryStream);
                researchmemoryStream.Position = 0;
                var research = await _cloudinary.UploadImageAsync(researchmemoryStream, Guid.NewGuid().ToString());

                var irbapprovalmemoryStream = new MemoryStream();
                request.IrbApproval.OpenReadStream().CopyTo(irbapprovalmemoryStream);
                irbapprovalmemoryStream.Position = 0;
                var irbapproval = await _cloudinary.UploadImageAsync(irbapprovalmemoryStream, Guid.NewGuid().ToString());

                if (!passport.Status || !degree.Status || !research.Status || !irbapproval.Status)
                {
                    return new BaseResponse(false, "An Error Occured WHile Processing Documents Please Contact Support After Many Tries");
                }
                var passportinfo = new Document
                {
                    Path = passport.Message,
                    Title = request.PassportPhoto.FileName,

                };

                var degreeinfo = new Document
                {
                    Path = degree.Message,
                    Title = request.Degree.FileName,

                };

                var researchproposalinfo = new Document
                {
                    Path = research.Message,
                    Title = request.ResearchProposal.FileName,

                };

                var irbapprovalinfo = new Document
                {
                    Path = irbapproval.Message,
                    Title = request.IrbApproval.FileName,

                };
                await _context.AddRangeAsync(degreeinfo, passportinfo,researchproposalinfo,irbapprovalinfo);
                await _context.SaveChangesAsync(cancellationToken);

                var researchcenter = new ResearchCenterInfo { 
                DegreeID = degreeinfo.Id,
                Institution = request.Institution,
                IRBApprovalID = irbapprovalinfo.Id,
                PassportID = passportinfo.Id,
                ResearchProposalID = researchproposalinfo.Id,
                Title = request.Title
                };

                await _context.AddAsync(researchcenter);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                var user = new DataAggregatorUser()
                {
                    Email = request.Email.ToLower(),
                    ResearchCenterInfo = researchcenter.ID,
                    AccountStatus = AccountStatusEnum.InActive,
                    FirstName = request.FirstName,
                    UserType = UserType.ResearchCenter,
                    TimeCreated = DateTime.UtcNow,
                    TimeUpdated = DateTime.UtcNow,
                    NormalizedEmail = request.Email!.ToUpperInvariant(),
                    NormalizedUserName = request.Email!.ToUpperInvariant(),
                    UserName = request.Email.ToLower()!,
                    LastName = request.LastName,
                    MiddleName = request.MiddleName,
                    signupsessionkey = Guid.NewGuid().ToString(),

                };

                await _context.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var sendemail = await _account.SendOTPAsync(new SendOTPRequest
                {
                    FirstName = request.FirstName!,
                    OtpCodeLength = OtpCodeLengthEnum.Six,
                    Purpose = OtpVerificationPurposeEnum.EmailConfirmation,
                    Recipient = request.Email,
                    RecipientType = OtpRecipientTypeEnum.Email,
                    UserId = user.Id

                }, cancellationToken);
                if (!sendemail.Status)
                {
                    return new BaseResponse(false, sendemail.Message!);
                }
                var addpassword = await _usermanager.AddPasswordAsync(user, request.Password!);
                if (!addpassword.Succeeded)
                    return new BaseResponse(false, "An Error Occured While Trying to Complete your Registration");
                var requesResearchCenter = new Request
                {
                    DateRequested = DateTimeOffset.UtcNow,
                    IsApproved = false,
                    Documents = $";{passportinfo.Id};{degreeinfo.Id};{researchproposalinfo.Id};{irbapprovalinfo.Id}",
                    UserID = user.Id,
                };
                await _context.AddAsync(requesResearchCenter, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                degreeinfo.UserID = user.Id;
                passportinfo.UserID = user.Id;
                researchproposalinfo.UserID = user.Id;
                irbapprovalinfo.UserID = user.Id;
                researchcenter.UserId = user.Id;

                _context.UpdateRange(degreeinfo, passportinfo, irbapprovalinfo,researchcenter,researchproposalinfo);
                await _context.SaveChangesAsync(cancellationToken);
               await transanction.CommitAsync(cancellationToken);
                return new BaseResponse(true, "Registration Successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"REGISTER_HOSPITAL_REQUEST =>something went wrong ");
                await transanction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                return new BaseResponse(false, "An Error Occured While Trying To COmplete Your Registration"); ;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"REGISTER_HOSPITAL_REQUEST =>something went wrong ");
            return new BaseResponse(false, "An Error Occured While Trying To COmplete Your Registration"); ;
        }
    }
}
