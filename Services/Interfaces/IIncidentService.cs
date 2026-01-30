using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Enums;
using cityWatch_Project.Models;

namespace cityWatch_Project.Services.Interfaces
{
    public interface IIncidentService
    {
        Task<string> CreateIncidentAsync(NewIncidentDto incidentDto);
        Task<List<Incident>> GetAllIncidentsAsync();
        Task<Incident> GetIncidentByIdAsync(Guid id);
        Task<string> DeleteIncidentByIdAsync(Guid id);
        Task<string> AssignWorkerToIncidentAsync(Guid incidentId, int workerId);
        Task<string> UpdateIncidentStatus(Guid incidentId, IncidentStatus status);
        Task<string> UpdateIncident(Guid incidentId, NewIncidentDto incidentDto, int reportedUserID);
    }
}
