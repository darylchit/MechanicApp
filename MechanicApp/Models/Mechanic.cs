using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Models;

public class Mechanic
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public int? ShopId { get; set; }
    
    [StringLength(100)]
    public string? Specialization { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public Shop? Shop { get; set; }
    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
}
