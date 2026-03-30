using System;

namespace BlogService.EventProcessing;

public interface IUserEventsProcessor
{
    void AddUser(string userPublishedMessage);
}
