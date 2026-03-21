namespace MechanicApp.DTOs;

public class CreateMechanicDto
{
    public string UserId { get; set; } = string.Empty;
    public int? ShopId { get; set; }
    public string? Specialization { get; set; }
    public bool IsAvailable { get; set; } = true;
}
