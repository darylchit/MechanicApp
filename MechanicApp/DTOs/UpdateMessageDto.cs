namespace MechanicApp.DTOs;

public class UpdateMessageDto
{
    public int ConversationId { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = "Sent";
}
