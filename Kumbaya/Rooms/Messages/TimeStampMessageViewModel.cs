using System;

namespace Kumbaya.Rooms.Messages
{
  public class TimeStampMessageViewModel : IMessageViewModel
  {
    public TimeStampMessageViewModel(DateTime createdAt)
    {
      CreatedAt = createdAt;
    }

    public DateTime CreatedAt { get; private set; }
  }
}