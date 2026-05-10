using Clinic.Api.Models.DTOs;
using Clinic.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto request)
        {
            await _appointmentService.CreateAsync(request);
            return Created(string.Empty, null);
        }

        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetByProfessional(Guid professionalId)
        {
            return Ok(await _appointmentService.GetByProfessionalAsync(professionalId));
        }
    }
}
