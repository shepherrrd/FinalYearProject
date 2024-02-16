using FinalYearProject.Api.Application.CQRS.Dashboard.Hospital;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [AuthorizeUserFilter(AuthorizationPolicyCodes.UserPolicyCode)]
    [ApiController]
    public class HospitalDashBoardController : ControllerBase
    {
        private readonly ISender _sender;
        public HospitalDashBoardController(ISender sender) => _sender = sender;

        [HttpGet("GetRequests")]
        public async Task<IActionResult> GetRequests() {
            var UserId = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(new GetHospitalDataRequests { UserID = UserId });
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPatch("ApproveRequest")]
        public async Task<IActionResult> AprroveRequest([FromBody] ChangeMedicalDataRequest request)
        {
            request.UserId = User?.Identity?.GetProfileId() ?? 0;
            request.IsApproved = true;
            var response = await _sender.Send(request);
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPatch("RejectRequest")]
        public async Task<IActionResult> RejectRequest([FromBody] ChangeMedicalDataRequest request)
        {
            request.UserId = User?.Identity?.GetProfileId() ?? 0;
            request.IsApproved = false;
            var response = await _sender.Send(request);
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("UploadData")]

        public async Task<IActionResult> UploadData([FromForm] UploadMedicalDataRequest request)
        {
            request.HospitalId = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(request);
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
