namespace MechanicApp.DTOs;

public class UpdateServiceRequestDto
{
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public int? MechanicId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? FinalCost { get; set; }
}
