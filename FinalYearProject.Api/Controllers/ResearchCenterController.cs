using FinalYearProject.Api.Application.CQRS.Dashboard.ResearchCenter;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchCenterController : ControllerBase
    {
        private readonly ISender _sender;

        public ResearchCenterController(ISender sender) => _sender = sender;
        [HttpGet("[action]")]
        public async Task<IActionResult> ResearchCenterDashboard()
        {
            var userID = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(new VIewResearchCenterDashboard { UserID = userID  });
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
