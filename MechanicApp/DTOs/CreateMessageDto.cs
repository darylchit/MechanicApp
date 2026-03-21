namespace MechanicApp.DTOs;

public class CreateMessageDto
{
    public int ConversationId { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
