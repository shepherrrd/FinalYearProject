using FinalYearProject.Api.Application.CQRS.Dashboard;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly ISender _sender;
        public DashBoardController(ISender sender) => _sender = sender;

        [HttpGet("GetRequests")]
        public async Task<IActionResult> GetRequests() {
            var UserId = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(new GetHospitalDataRequests { UserID = UserId });
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPatch("ApproveRequest/:id")]
        public async Task<IActionResult> AprroveRequest([FromRoute] int id)
        {
            var UserId = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(new ChangeHospitalRequestStatus { HospitalId= UserId,IsApproved= true,RequestId = id });
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPatch("RejectRequest/:id")]
        public async Task<IActionResult> RejectRequest([FromRoute] int id)
        {
            var UserId = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(new ChangeHospitalRequestStatus { HospitalId= UserId,IsApproved= false,RequestId = id });
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
