namespace cityWatch_Project.DTOs.Incidents
{
    public class IncidentResponseDto<T>
    {
        public string? ErrorMessage { get; set; }
        public bool Error {  get; set; }
        public T? Data { get; set; }
    }
}
