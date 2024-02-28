using FinalYearProject.Api.Application.CQRS.Dashboard.Admin;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ISender _sender;

        public AdminController(ISender sender) => _sender = sender;

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllRegistrationRequests()
        {
            var response = await _sender.Send(new ViewRegistrationRequests { AdminID = User?.Identity?.GetProfileId() ?? 0 });
            if(!response.Status)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> ChangeRequestStatus([FromBody] UpdateRegistrationStatusRequest request)
        {
            request.AdminID = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(request);
            if(!response.Status)
                return BadRequest(response);
            return Ok(response);    
        }
    }
}
