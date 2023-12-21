using FinalYearProject.Api.Application.CQRS.Registration;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegistrationController : ControllerBase
    {
        private readonly ISender _sender;
        public RegistrationController(ISender sender) => _sender = sender;

        [HttpPost("RegisterHospital")]
        public async Task<IActionResult> RegisterHospital([FromForm] RegisterHospitalRequest request)
        {
            var response = await _sender.Send(request);
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpPost("RegisterResearchCenter")]
        public async Task<IActionResult> RegisterResearchCenter([FromForm] RegisterResearchCenterRequest request)
        {
            var response = await _sender.Send(request);
            if (!response.Status)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
