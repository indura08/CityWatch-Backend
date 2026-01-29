using cityWatch_Project.Enums;

namespace cityWatch_Project.Models
{
    public class Incident
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public IncidentCategory Category { get; set; }
        public IncidentStatus Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public IncidentLocationSource LocationSource { get; set; }
        public bool IsLocationVerified { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
        public int ReportedByUserId { get; set; }
        public User? ReportedBy { get; set; }
        public int? AssignedToUserID { get; set; }
        public User? AssignedTo { get; set; }


    }
}
