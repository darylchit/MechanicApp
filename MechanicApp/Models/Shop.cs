using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Models;

public class Shop
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Address { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<Mechanic> Mechanics { get; set; } = new List<Mechanic>();
}
