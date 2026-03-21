namespace MechanicApp.DTOs;

public class CreateConversationDto
{
    public int ClientId { get; set; }
    public int MechanicId { get; set; }
    public int? ServiceRequestId { get; set; }
    public string? Subject { get; set; }
}
