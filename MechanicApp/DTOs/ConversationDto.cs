namespace MechanicApp.DTOs;

public class ConversationDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int MechanicId { get; set; }
    public int? ServiceRequestId { get; set; }
    public string? Subject { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    
    // Related data
    public string? ClientName { get; set; }
    public string? MechanicName { get; set; }
}
