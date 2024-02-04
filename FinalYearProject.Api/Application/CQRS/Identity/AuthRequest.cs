using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Auth.JWT;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Api.Application.CQRS.Identity;

public class AuthRequest : IRequest<BaseResponse<LoginResponse>>
{
#nullable disable
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<AuthRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}

public class LoginRequestHandler : IRequestHandler<AuthRequest, BaseResponse<LoginResponse>>
{
    private readonly FinalYearDBContext _context;
    private  SignInManager<DataAggregatorUser> _signinmanager;
    private  UserManager<DataAggregatorUser> _usermanager;
    private readonly ILogger<LoginRequestHandler> _logger;
    private readonly IJwtHandler _jwtHandler;

    public LoginRequestHandler(SignInManager<DataAggregatorUser> signinmanager,
        UserManager<DataAggregatorUser> usermanager,
        FinalYearDBContext context,
        ILogger<LoginRequestHandler> logger,
        IJwtHandler jwtHandler
        )
    {
        _signinmanager = signinmanager;
        _usermanager = usermanager;
        _context = context;
        _logger = logger;
        _jwtHandler = jwtHandler;
    }

    public async Task<BaseResponse<LoginResponse>> Handle(AuthRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.Where(x => x.Email.ToLower() == request.Email.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if(user is null)
            {
                return new BaseResponse<LoginResponse>(false, "A user tied to this email was not found");
            }
           var ff = await _usermanager.CheckPasswordAsync(user, request.Password);
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await _signinmanager.PasswordSignInAsync(user, request.Password, true, true);
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        _logger?.LogInformation($"LOGIN_REQUEST => Process cancelled | Account is locked");

                        user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(1000);
                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync();
                        return new BaseResponse<LoginResponse>(false, "Your account has been locked. Please initiate a password reset to unlock your account.");
                    }
                    else
                    {
                        await _usermanager.AccessFailedAsync(user);
                        int maxAttempts = _usermanager.Options.Lockout.MaxFailedAccessAttempts;
                        int failedAttempts = user.AccessFailedCount;
                        if (maxAttempts - failedAttempts == 1)
                        {
                            user.LockoutEnabled = true;
                            user.LockoutEnd = DateTime.UtcNow.AddYears(1000);
                            await _context.SaveChangesAsync(cancellationToken);
                            await transaction.CommitAsync();
                            return new BaseResponse<LoginResponse>(false, "Your account has been locked. Please initiate a password reset to unlock your account.");
                        }

                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync();
                        _logger?.LogInformation($"LOGIN_REQUEST => Process cancelled | Invalid email and password combination");
                        return new BaseResponse<LoginResponse>(false, $"You have provided wrong login credentials, please try again. You have {maxAttempts - 1 - failedAttempts} login attempts left.");
                    }
                }

                // reset lockout count
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.LockoutEnd = DateTimeOffset.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                
                if (!user.EmailConfirmed)
                {
                    await transaction.CommitAsync();

                    return new BaseResponse<LoginResponse>(false, "Your Account hasn't been verified please go back and verify your accoutn");
                }

                if (user.AccountStatus is not AccountStatusEnum.Active)
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<LoginResponse>(false, $"Your Account is not active, your account is {user.AccountStatus.GetDescription()}, please contact support for help");
                }

                var loginResponse = _jwtHandler.Create(new JwtRequest
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    EmailAddress = user.Email,
                    IsEmailVerified = user.EmailConfirmed,
                    UserType = user.UserType,
                    AccountStatus = user.AccountStatus,
                    ProfileCode = AuthorizationPolicyCodes.UserPolicyCode,
                });
                await transaction.CommitAsync(cancellationToken);
                return new BaseResponse<LoginResponse>(true, "Signin Successfull", loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"something went wrong");
                await transaction.RollbackAsync(cancellationToken);
                return new BaseResponse<LoginResponse>(false, "An Error Occured while trying to process your login request");
            }
            
            }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"something went wrong");
            return new BaseResponse<LoginResponse>(false, "An Error Occured while trying to process your login request");
        }

    }
}
