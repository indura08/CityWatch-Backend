using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Models;

namespace cityWatch_Project.Repositories.Interfaces
{
    public interface IIncidentRepository
    {
        Task CreateIncidentAsync(Incident incident);
        Task<List<Incident>> GetAllIncidentsAsync();
        Task<Incident> GetIncidentByIdAsync(Guid id);
        //Task UpdateIncident(Incident incident);
        Task DeleteIncidentByIdAsync(Guid id);
    }
}
