using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Enums;
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

        [HttpGet("all")]
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

        [HttpPost("create")]
        [Authorize(Roles = "Admin,Citizen,Worker")]
        public async Task<ActionResult<IncidentResponseDtoNoData>> CreateNewIncidentAsync(NewIncidentDto incidentDto)
        {
            var result = await _incidentService.CreateIncidentAsync(incidentDto);
            if (result.Success == false)
            {
                return BadRequest(new IncidentResponseDtoNoData
                {
                    Error = true,
                    Message = result.Message
                });
            }

            return Ok(new IncidentResponseDtoNoData
            {
                Error = false,
                Message = result.Message
            });

        }

        [HttpGet("assign/worker")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IncidentResponseDto<string>>> AssignWorkerToIncident(Guid incidentId, int WorkerId)
        {
            var result = await _incidentService.AssignWorkerToIncidentAsync(incidentId, WorkerId);

            if(result.Success == false)
            {
                return BadRequest(new IncidentResponseDto<string>
                {
                    Error = true,
                    ErrorMessage = result.Message,
                    Data = null
                });
            }

            return Ok(new IncidentResponseDto<string>
            {
                Error = false,
                ErrorMessage = null,
                Data = result.Message
            });
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin,Citizen")]
        public async Task<ActionResult<IncidentResponseDto<string>>> DeleteIncidentById(Guid id, int userId)
        {
            var result = await _incidentService.DeleteIncidentByIdAsync(id, userId);
            if (result.Success == false) return BadRequest(new IncidentResponseDto<string>
            {
                Error = true,
                ErrorMessage = result.Message,
                Data = null
            });
            
            return Ok(new IncidentResponseDto<string>
            {
                Error = false,
                ErrorMessage = null,
                Data = result.Message
            });
        }

        [HttpPost("update/status/{id}")]
        [Authorize(Roles = "Admin,Worker")]
        public async Task<ActionResult<IncidentResponseDto<string>>> UpdateIncidentStatus(Guid id, IncidentStatus status)
        {
            var result = await _incidentService.UpdateIncidentStatus(id, status);
            if (result.Success == false) return BadRequest(new IncidentResponseDto<string>
            {
                Error = true,
                ErrorMessage = result.Message,
                Data = null
            });

            return Ok(new IncidentResponseDto<string>
            {
                Error = false,
                ErrorMessage = null,
                Data = result.Message
            });
        }

        public async Task<ActionResult<IncidentResponseDto<string>>> UpdateIncident(Guid id, NewIncidentDto incidentDto, int userId)
        {
            var result = await _incidentService.UpdateIncident(id, incidentDto, userId);
            if (result.Success == false) return BadRequest(new IncidentResponseDto<string>
            {
                Error = false,
                ErrorMessage = result.Message,
                Data = null
            });

            return Ok(new IncidentResponseDto<string>
            {
                Error = false,
                ErrorMessage = null,
                Data = result.Message
            });
        }

    }
}
