namespace BlogService.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}
