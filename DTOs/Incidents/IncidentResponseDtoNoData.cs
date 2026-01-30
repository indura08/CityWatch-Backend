using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cityWatch_Project.DTOs.Incidents
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentResponseDtoNoData : ControllerBase
    {
        public bool Error { get; set; }
        public string? Message { get; set; }

    }
}
