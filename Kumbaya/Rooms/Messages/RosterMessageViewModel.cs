using GrayHills.Matches;

namespace Kumbaya.Rooms.Messages
{
  public class RosterMessageViewModel : IMessageViewModel
  {
    private readonly MessageType type;

    public RosterMessageViewModel(string name, MessageType type)
    {
      this.Name = name;
      this.type = type;
    }

    public string Name { get; private set; }
    public string Message
    {
      get
      {
        return type == MessageType.Enter
                 ? "has entered the room"
                 : "has left the room";
      }
    }
  }
}