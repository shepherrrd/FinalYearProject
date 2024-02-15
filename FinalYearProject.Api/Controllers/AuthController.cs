
using FinalYearProject.Api.Application.CQRS.Identity;
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        public AuthController(ISender sender) => _sender = sender;

        [HttpPost("SignIn")]
        public async Task<IActionResult> SingIn([FromBody] AuthRequest request)
        {
            var response = await _sender.Send(request);
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [AuthorizeUserFilter(policies: AuthorizationPolicyCodes.UserPolicyCode)]
        public async Task<IActionResult> ping()
        {
            return Ok(User?.Identity?.GetProfileId() ?? 0);
        }
    }
}
