using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Models;

public class ServiceRequest
{
    public int Id { get; set; }
    
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    public int VehicleId { get; set; }
    
    public int? MechanicId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public ServiceStatus Status { get; set; } = ServiceStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ScheduledAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public decimal? EstimatedCost { get; set; }
    
    public decimal? FinalCost { get; set; }
    
    // Navigation properties
    public Client Client { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public Mechanic? Mechanic { get; set; }
    public Conversation? Conversation { get; set; }
}
