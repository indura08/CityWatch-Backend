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
        public async Task<IncidentServiceResponse> CreateIncidentAsync(NewIncidentDto incidentDto)
        {
            if (incidentDto == null) return new IncidentServiceResponse 
            {
                Success = false,
                Message = "Please enter values to adda new incident"
            };

            Console.WriteLine($"ReportedByUserId = {incidentDto.ReportedByUserId}");

            var ReportedUser = await _userRepository.FindUserByIdAsync(incidentDto.ReportedByUserId);
            if (ReportedUser == null) return new IncidentServiceResponse 
            {
                Success = false,
                Message = "Reported user cannot be found"
            };

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
                //ReportedBy = ReportedUser,
            };

            if (incidentDto.Image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "IncidentImages");
                string fileName = incidentDto.Image.FileName;
                Console.WriteLine($"file name is : ${fileName}");
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    incidentDto.Image.CopyTo(fileStream);
                }

                incident.ImageUrl = filePath;
            }

            await _incidentRepository.CreateIncidentAsync(incident);

            return new IncidentServiceResponse 
            {
                Success = true,
                Message = "Incident Created Successfully"
            };
        }

        public async Task<IncidentServiceResponse> DeleteIncidentByIdAsync(Guid id, int? userId)
        {
            var currentIncident = await _incidentRepository.GetIncidentByIdAsync(id);
            if (currentIncident == null) return new IncidentServiceResponse 
            {
                Success = false,
                Message = "Incident cannot be found"
            };

            if (userId != null && currentIncident.ReportedByUserId != userId) return new IncidentServiceResponse
            {
                Success = false,
                Message = "You are not the one who reported the incident, hence you cannot delete this"
            };

            await _incidentRepository.DeleteIncidentByIdAsync(id);
            return new IncidentServiceResponse 
            {
                Success = true,
                Message = "Incident successfully deleted"
            };
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

        public async Task<IncidentServiceResponse> AssignWorkerToIncidentAsync(Guid incidentId, int workerId) 
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(incidentId);
            if (incident == null) return new IncidentServiceResponse 
            {
                Success = false,
                Message = "Incident could not be found"
            };

            var workerUser = await _userRepository.FindUserByIdAsync(workerId);

            if (workerUser == null) return new IncidentServiceResponse 
            {
                Success = true,
                Message = $"Worker with id: {workerId} user cannot be found"
            };
            if (workerUser.Role != Enums.Role.Worker) return new IncidentServiceResponse 
            {
                Success = false,
                Message = "Selected user is not a worker, please check again"
            };

            incident.AssignedToUserID = workerUser.UserID;
            incident.LastUpdatedAt = DateTime.UtcNow;
            await _incidentRepository.SaveChangesAsync();

            return new IncidentServiceResponse 
            {
                Success = true,
                Message = $"Worker user with id: {workerId} assigned to the incident successfully"
            };
        }

        public async Task<IncidentServiceResponse> UpdateIncidentStatus(Guid incidentId, IncidentStatusChangeDto incidentStatus) 
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(incidentId);
            if (incident == null) return new IncidentServiceResponse 
            {
                Success = false,
                Message = "Incident cannot be found"
            };

            incident.Status = incidentStatus.Status;
            incident.LastUpdatedAt = DateTime.UtcNow;

            await _incidentRepository.SaveChangesAsync();
            return new IncidentServiceResponse
            {
                Success = true,
                Message = "Incident status changed successfully"
            };

        }

        public async Task<IncidentServiceResponse> UpdateIncident(Guid incidentId, NewIncidentDto incidentDto, int? reportedUserId)
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(incidentId);
            if (incident == null) return new IncidentServiceResponse
            {
                Success = false,
                Message = $"Incident with id {incidentId} couldn't be found"
            };

            if (reportedUserId != null && incident.ReportedByUserId != reportedUserId) return new IncidentServiceResponse
            {
                Success = false,
                Message = "You cannot edit this since you are not the user who reported this"
            };

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
            return new IncidentServiceResponse
            {
                Success = true,
                Message = "Incident succesfully updated"
            };
        }
    }
}

