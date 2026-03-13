namespace MechanicApp.Models;

public enum ServiceStatus
{
    Pending,
    Scheduled,
    InProgress,
    AwaitingParts,
    Completed,
    Cancelled,
    OnHold
}
