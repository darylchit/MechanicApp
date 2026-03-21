using MechanicApp.Models;

namespace MechanicApp.DTOs;

public class ServiceRequestDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public int? MechanicId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? FinalCost { get; set; }
    
    // Related data
    public string? VehicleMake { get; set; }
    public string? VehicleModel { get; set; }
    public string? MechanicName { get; set; }
}
