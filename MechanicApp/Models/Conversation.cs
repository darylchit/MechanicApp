using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Models;

public class Conversation
{
    public int Id { get; set; }
    
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    public int MechanicId { get; set; }
    
    public int? ServiceRequestId { get; set; }
    
    [StringLength(200)]
    public string? Subject { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastMessageAt { get; set; }
    
    // Navigation properties
    public Client Client { get; set; } = null!;
    public Mechanic Mechanic { get; set; } = null!;
    public ServiceRequest? ServiceRequest { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
