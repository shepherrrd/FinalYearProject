using FinalYearProject.Api.Application.CQRS.Registration;
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinalYearProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegistrationController : ControllerBase
    {
        private readonly ISender _sender;
        public RegistrationController(ISender sender) => _sender = sender;

        /// <summary>
        /// Handle Hospital registration
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A baseresponse of object</returns>
        /// <response code="200">Operation successful</response>
        /// <response code="400">If validation fails due to validation errors or application encountered an exception</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
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
