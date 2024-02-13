using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
    public VerifyAccountOtpRequestHandler(IAccountService accountService,
        FinalYearDBContext context
        )
    {
        _accountService = accountService;
        _context = context;
    }

    public async Task<BaseResponse> Handle(VerifyAccountOtpRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.signupsessionkey == request.signupsessionkey);
        if (user is null)
        {
            return new BaseResponse(false, "No User was found");
        }
        var validateotp = await _accountService.ValidateOTPCodeAsync(new ValidateOtpRequest
        {
            Code = request.signupsessionkey,
            Purpose = OtpVerificationPurposeEnum.EmailConfirmation,
            UserId = user.Id
        },cancellationToken);
        if (!validateotp.Status)
        {
            return new BaseResponse(false,validateotp.Message ?? "Invalid Otp");
        }
        user.AccountStatus = AccountStatusEnum.Active;
        user.TimeUpdated = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return new BaseResponse(true, "Account Verified Successfully, Now and Admin will verify your details shortly");
    }
}
