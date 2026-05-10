using Clinic.Api.Models.DTOs;
using Clinic.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfessionalsController : ControllerBase
    {
        private readonly IProfessionalService _professionalService;

        public ProfessionalsController(IProfessionalService professionalService)
        {
            _professionalService = professionalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _professionalService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfessionalDto request)
        {
            var professional = await _professionalService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = professional.Id }, professional);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var professional = await _professionalService.GetByIdAsync(id);
            if (professional == null)
            {
                return NotFound();
            }
            return Ok(professional);
        }
    }
}
