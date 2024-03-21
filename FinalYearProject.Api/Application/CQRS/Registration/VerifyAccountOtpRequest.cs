using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalYearProject.Api.Application.CQRS.Registration;

public class VerifyAccountOtpRequest : IRequest<BaseResponse>
{
    public string signupsessionkey { get; set; } = default!;
    public string Code { get; set; } = default!;
}

public class VerifyAccountOtpRequestValidator : AbstractValidator<VerifyAccountOtpRequest>
{
    public VerifyAccountOtpRequestValidator()
    {
        RuleFor(x => x.signupsessionkey).NotNull().NotEmpty();
        RuleFor(x => x.Code).NotNull().NotEmpty();
    }
}

public class VerifyAccountOtpRequestHandler : IRequestHandler<VerifyAccountOtpRequest, BaseResponse>
{
    private readonly IAccountService _accountService;
    private readonly FinalYearDBContext _context;
    private readonly IEncryptionService _utilityService;
    public VerifyAccountOtpRequestHandler(IAccountService accountService,
        FinalYearDBContext context,
        IEncryptionService utilityService 
        )
    {
        _accountService = accountService;
        _context = context;
        _utilityService = utilityService;
    }

    public async Task<BaseResponse> Handle(VerifyAccountOtpRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.signupsessionkey == request.signupsessionkey,cancellationToken);
        if (user is null)
        {
            return new BaseResponse(false, "No User was found");
        }
        var validateotp = await _accountService.ValidateOTPCodeAsync(new ValidateOtpRequest
        {
            Code = request.Code,
            Purpose = OtpVerificationPurposeEnum.EmailConfirmation,
            UserId = user.Id
        },cancellationToken);
        if (!validateotp.Status)
        {
            return new BaseResponse(false,validateotp.Message ?? "Invalid Otp");
        }
        user.AccountStatus = AccountStatusEnum.PendingApproval;
        user.EmailConfirmed = true;
        user.TimeUpdated = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        if(user.AccountStatus == AccountStatusEnum.Active && user.UserType == UserType.Hospital)
        {
            var keys =  _utilityService.GenerateEncryptionKey();
            var userrsa = new UserRSA { 
            PrivateRSAParameters = JsonConvert.SerializeObject( keys.Data!.privatersa),
            PublicRSAParameters = JsonConvert.SerializeObject( keys.Data.publicrsa),
            UserID = user.Id,
            };
            await _context.AddAsync(userrsa,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var sendkeys = await _accountService.SendHospitalWelcomeEmailAsync(new SendHospitalKeysRequest {
                Email = user.Email,
                FirstName = user.FirstName,
                PrivateKeyExponent = keys.Data!.PrivateKeyExponent,
                PublicKeyExponent = keys.Data.PublicKeyExponent,
                PrivateKeyModulus = keys.Data.PrivateKeyModulus,
                PublicKeyModulus = keys.Data.PublicKeyModulus,
                UserId = user.Id
            
            }, cancellationToken);
        }
        return new BaseResponse(true, "Account Verified Successfully, Now and Admin will verify your details shortly");
    }
}
