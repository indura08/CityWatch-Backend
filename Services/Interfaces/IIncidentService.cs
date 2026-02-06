using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Enums;
using cityWatch_Project.Models;

namespace cityWatch_Project.Services.Interfaces
{
    public interface IIncidentService
    {
        Task<IncidentServiceResponse> CreateIncidentAsync(NewIncidentDto incidentDto);
        Task<List<Incident>> GetAllIncidentsAsync();
        Task<Incident> GetIncidentByIdAsync(Guid id);
        Task<IncidentServiceResponse> DeleteIncidentByIdAsync(Guid id, int? userId);
        Task<IncidentServiceResponse> AssignWorkerToIncidentAsync(Guid incidentId, int workerId);
        Task<IncidentServiceResponse> UpdateIncidentStatus(Guid incidentId, IncidentStatusChangeDto status);
        Task<IncidentServiceResponse> UpdateIncident(Guid incidentId, NewIncidentDto incidentDto, int? reportedUserID);
    }
}
