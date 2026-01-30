using cityWatch_Project.Enums;
using cityWatch_Project.Models;

namespace cityWatch_Project.DTOs.Incidents
{
    public class NewIncidentDto
    {
        
        public string? Description { get; set; }
        public IncidentCategory Category { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public IncidentLocationSource LocationSource { get; set; }
        public bool IsLocationVerified { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int ReportedByUserId { get; set; }
        public int? AssignedToUserID { get; set; }
    }
}
