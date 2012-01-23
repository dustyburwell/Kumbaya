using System.Windows.Media;

namespace Kumbaya.Rooms.Messages
{
  public class MessageViewModel : IMessageViewModel
  {
    private readonly bool isMe;

    public MessageViewModel(string name, string body, bool isMe)
    {
      Name = name;
      Body = body;
      this.isMe = isMe;
    }

    public string Name { get; private set; }
    public string Body { get; private set; }

    public Brush NameBackground
    {
      get
      {
        return isMe
                 ? MeBrush
                 : Brushes.LightGray;
      }
    }
    
    public Brush MessageBackground
    {
      get
      {
        return isMe
                 ? MeBrush
                 : Brushes.Transparent;
      }
    }

    private static Brush meBrush;
    private static Brush MeBrush
    {
      get { return meBrush ?? (meBrush = new SolidColorBrush(Color.FromRgb(255, 255, 200))); }
    }
  }
}