using cityWatch_Project.Data;
using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Models;
using cityWatch_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cityWatch_Project.Repositories.Implementations
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly MainDBContext _dbContext;

        public IncidentRepository(MainDBContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task CreateIncidentAsync(Incident incident)
        {
            if (incident == null) return;

            await _dbContext.Incident.AddAsync(incident);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteIncidentByIdAsync(Guid id)
        {
            await _dbContext.Incident.Where(i => i.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Incident>> GetAllIncidentsAsync()
        {
            var Incidents = await _dbContext.Incident.ToListAsync();
            return Incidents;
        }

        public async Task<Incident> GetIncidentByIdAsync(Guid id)
        {
            //meke find use kalam performance wadi mokda meka PK eknma check kerena nisa , first or default danna one nm onema property ekk ekkala puluwan hinda eka poddk performance madi
             var incident = await _dbContext.Incident.FindAsync(id);

            if (incident == null) return null;
            return incident;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
