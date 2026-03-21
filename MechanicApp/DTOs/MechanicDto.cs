namespace MechanicApp.DTOs;

public class MechanicDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? ShopId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Specialization { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Related data
    public string? ShopName { get; set; }
}
