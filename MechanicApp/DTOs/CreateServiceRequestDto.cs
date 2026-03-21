namespace MechanicApp.DTOs;

public class CreateServiceRequestDto
{
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public int? MechanicId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public decimal? EstimatedCost { get; set; }
}
