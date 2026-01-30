using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Models;
using cityWatch_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cityWatch_Project.Controllers
{
    [Route("api/incident")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;

        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [HttpGet]
        public async Task<ActionResult<IncidentResponseDto<List<Incident>>>> GetAllIncidentsAsync()
        {
            var incidentList = await _incidentService.GetAllIncidentsAsync();

            return Ok(new IncidentResponseDto<List<Incident>>
            {
                Data = incidentList,
                Error = false,
                ErrorMessage = null
            });
        }

        public async Task<ActionResult<IncidentResponseDtoNoData>> CreateNewIncidentAsync(NewIncidentDto incidentDto)
        {
            var message = await _incidentService.CreateIncidentAsync(incidentDto);
            if (message == "No data to create a new incident" || message == "Reported user cannot be found")
            {
                return BadRequest(new IncidentResponseDtoNoData
                {
                    Error = true,
                    Message = message
                });
            }

            return Ok(new IncidentResponseDtoNoData
            {
                Error = false,
                Message = message
            });

        }
    }
}
