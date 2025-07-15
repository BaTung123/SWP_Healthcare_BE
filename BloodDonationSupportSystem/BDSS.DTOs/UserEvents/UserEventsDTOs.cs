namespace BDSS.DTOs.UserEvents;

public class RegisterUserToEventRequest
{
    public long UserId { get; set; }
    public long EventId { get; set; }
}

public class RegisterUserToEventResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}