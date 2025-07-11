namespace BDSS.DTOs.Event;

using BDSS.Common.Enums;

public class EventDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string LocationAddress { get; set; } = string.Empty;
    public int TargetParticipant { get; set; }
    public EventStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateEventRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string LocationAddress { get; set; } = string.Empty;
    public int TargetParticipant { get; set; }
    public EventStatus Status { get; set; }
}

public class UpdateEventRequest
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string LocationAddress { get; set; } = string.Empty;
    public int TargetParticipant { get; set; }
    public EventStatus Status { get; set; }
}

public class DeleteEventRequest
{
    public long Id { get; set; }
}

public class GetEventByIdRequest
{
    public long Id { get; set; }
}

public class GetAllEventsResponse
{
    public IEnumerable<EventDto> Events { get; set; } = new List<EventDto>();
}