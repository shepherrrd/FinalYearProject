using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Microsoft.Extensions.Logging;

namespace FinalYearProject.Infrastructure.Infrastructure.Auth
{
    public class AuthorizeUserFilter : TypeFilterAttribute
    {
        public AuthorizeUserFilter(string policies) : base(typeof(AuthorizeTokenFilter))
        {
            Arguments = new object[] { policies };
        }

        public class AuthorizeTokenFilter : IAsyncAuthorizationFilter
        {
            private readonly IAuthorizationService _authorization;
            public readonly FinalYearDBContext _context;
            private readonly ILogger<AuthorizeTokenFilter> _logger;

            public string _policies { get; set; }

            public AuthorizeTokenFilter(string policies, FinalYearDBContext context,ILogger<AuthorizeTokenFilter> logger, IAuthorizationService authorization)
            {
                _context = context;
                _logger = logger;
                _authorization = authorization;
                _policies = policies;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var policies = _policies.Split(";").ToList();
                long userId = context.HttpContext.User.Identity?.GetProfileId() ?? 0;
                string token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? "";

                try
                {
                    int accountStatus = context.HttpContext.User.Identity!.GetAccountStatus();
                    if (accountStatus != (int)AccountStatusEnum.Active)
                    {
                        var errorResponse = new ErrorResponse
                        {
                            Status = false,
                            Message = $"Account has been {((AccountStatusEnum)accountStatus).GetDescription()}. Please contact support."
                        };
                        context.Result = new UnauthorizedObjectResult(errorResponse);
                        return;
                    }
                    foreach (var policy in policies)
                    {
                        var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                        if (authorized.Succeeded)
                        {
                            return;
                        }
                    }
                    context.Result = new UnauthorizedObjectResult(new ErrorResponse { Status = false, Message = $"Session exipired, login to continue" });
                    //context.Result = new UnauthorizedResult();
                }
                catch (Exception ex)
                {
                    context.Result = new UnauthorizedObjectResult(new ErrorResponse { Status = false, Message = $"Application error, unable to authorize current error." });
                    _logger.LogError(ex, "Application error, unable to authorize current error.");
                    return;
                }
            }
        }
    }
}