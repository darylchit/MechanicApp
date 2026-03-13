using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Models;

public class Message
{
    public int Id { get; set; }
    
    [Required]
    public int ConversationId { get; set; }
    
    [Required]
    public string SenderId { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public MessageStatus Status { get; set; } = MessageStatus.Sent;
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ReadAt { get; set; }
    
    // Navigation properties
    public Conversation Conversation { get; set; } = null!;
    public ApplicationUser Sender { get; set; } = null!;
}
