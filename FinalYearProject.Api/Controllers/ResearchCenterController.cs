using FinalYearProject.Api.Application.CQRS.Dashboard.ResearchCenter;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpPost("[action]")]

        public async Task<IActionResult> AddMedicalDataRequest([FromForm] AddMedicalDataRequest request)
        {
            request.UserID = User?.Identity?.GetProfileId() ?? 0;
            var response = await _sender.Send(request);
            if(!response.Status)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("[action]/{type}")]

        public async Task<IActionResult> GetMedicalRecords([FromRoute]MedicalRecordTypeEnum type)
        {
            var userID = User?.Identity?.GetProfileId() ?? 0;    
            var response = await _sender.Send(new GetMedicalRecordsFromType { MedicalRecordType = type , UserID = userID});
            if(!response.Status)
                return BadRequest(response);
            return Ok(response);
        
        }


    }
}
