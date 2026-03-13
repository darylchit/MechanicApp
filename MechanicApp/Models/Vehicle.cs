using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Models;

public class Vehicle
{
    public int Id { get; set; }
    
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Make { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Model { get; set; } = string.Empty;
    
    public int Year { get; set; }
    
    [StringLength(20)]
    public string? LicensePlate { get; set; }
    
    [StringLength(17)]
    public string? VIN { get; set; }
    
    [StringLength(30)]
    public string? Color { get; set; }
    
    public int? Mileage { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Client Client { get; set; } = null!;
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
