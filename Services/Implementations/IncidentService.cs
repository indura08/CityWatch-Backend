using cityWatch_Project.DTOs.Incidents;
using cityWatch_Project.Enums;
using cityWatch_Project.Models;
using cityWatch_Project.Repositories.Interfaces;
using cityWatch_Project.Services.Interfaces;

namespace cityWatch_Project.Services.Implementations
{
    public class IncidentService : IIncidentService
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IncidentService(IIncidentRepository incidentrepo, IWebHostEnvironment webHostEnv, IUserRepository userrepo) 
        {
            _incidentRepository = incidentrepo;
            _userRepository = userrepo;
            _webHostEnvironment = webHostEnv;
        }
        public async Task<string> CreateIncidentAsync(NewIncidentDto incidentDto)
        {
            if (incidentDto == null) return "No data to create a new incident";

            var ReportedUser = await _userRepository.FindUserByIdAsync(incidentDto.ReportedByUserId);
            if (ReportedUser == null) return "Reported user cannot be found";

            Incident incident = new Incident
            {
                Id = Guid.NewGuid(),
                Description = incidentDto.Description,
                Category = incidentDto.Category,
                Latitude = incidentDto.Latitude,
                Longitude = incidentDto.Longitude,
                Address = incidentDto.Address,
                Status = Enums.IncidentStatus.ACTIVE,
                LocationSource = incidentDto.LocationSource,
                IsLocationVerified = incidentDto.IsLocationVerified,
                ReportedByUserId = incidentDto.ReportedByUserId,
                ReportedBy = ReportedUser,
            };

            if (incidentDto.Image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string fileName = incidentDto.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    incidentDto.Image.CopyTo(fileStream);
                }

                incident.ImageUrl = filePath;
            }

            return "Incident created";
        }

        public async Task<string> DeleteIncidentByIdAsync(Guid id)
        {
            var currentIncident = await _incidentRepository.GetIncidentByIdAsync(id);
            if (currentIncident == null) return "Incident cannot be found";

            await _incidentRepository.DeleteIncidentByIdAsync(id);
            return "Incident successfully deleted";
        }

        public async Task<List<Incident>> GetAllIncidentsAsync()
        {
            var incidentList = await _incidentRepository.GetAllIncidentsAsync();
            return incidentList;
        }

        public async Task<Incident> GetIncidentByIdAsync(Guid id)
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(id);
            return incident;
        }

        public async Task<string> AssignWorkerToIncidentAsync(Guid incidentId, int workerId) 
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(incidentId);
            if (incident == null) return "Incident could not be found";

            var workerUser = await _userRepository.FindUserByIdAsync(workerId);

            if (workerUser == null) return $"Worker with id: {workerId} user cannot be found";
            if (workerUser.Role != Enums.Role.Worker) return "Selected user is not a worker, please check again";

            incident.AssignedTo = workerUser;
            await _incidentRepository.SaveChangesAsync();

            return $"Worker user with id: {workerId} assigned to the incident successfully";
        }

        public async Task<string> UpdateIncidentStatus(Guid incidentId, IncidentStatus incidentStatus) 
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(incidentId);
            if (incident == null) return "Incident cannot be found";

            incident.Status = incidentStatus;
            incident.LastUpdatedAt = DateTime.UtcNow;

            await _incidentRepository.SaveChangesAsync();
            return "Incident status changed successfully";

        }

        public async Task<string> UpdateIncident(Guid incidentId, NewIncidentDto incidentDto, int reportedUserId)
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(incidentId);
            if (incident == null) return $"Incident with id {incidentId} couldn't be found";
            if (incident.ReportedByUserId != reportedUserId) return "You cannot edit this since you are not the user who reported this";

            incident.Description = incidentDto.Description;
            incident.LastUpdatedAt = DateTime.UtcNow;
            incident.LocationSource = incidentDto.LocationSource;
            incident.Category = incidentDto.Category;
            incident.Latitude = incidentDto.Latitude;
            incident.Longitude = incidentDto.Longitude;
            incident.Address = incidentDto.Address;
            incident.IsLocationVerified = incidentDto.IsLocationVerified;
            //anything else cannot be updated due to business logic i have created.

            await _incidentRepository.SaveChangesAsync();
            return "Incident succesfully updated";
        }
    }
}
