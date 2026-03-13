using Microsoft.AspNetCore.Identity;

namespace MechanicApp.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Client? Client { get; set; }
    public Mechanic? Mechanic { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
