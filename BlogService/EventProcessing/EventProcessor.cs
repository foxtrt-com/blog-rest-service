using System.Text.Json;
using AutoMapper;
using BlogService.Dtos;

namespace BlogService.EventProcessing;

public class EventProcessor : IEventProcessor
{

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.UserPublished:

            default:
                return;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        if (eventType == null) { return EventType.Undetermined; }

        switch (eventType.Event)
        {
            case "User_Published":
                Console.WriteLine("User Publish Event Detected");
                return EventType.UserPublished;
            default:
                Console.WriteLine("Unknown Publish Event Detected");
                return EventType.Undetermined;
        }
    }
}

enum EventType
{
    UserPublished,
    Undetermined
}