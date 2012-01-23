using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using GrayHills.Matches;
using Kumbaya.Rooms.Messages;
using Message = GrayHills.Matches.Message;

namespace Kumbaya.Rooms
{
  public class RoomViewModel : Screen
  {
    private readonly Room room;
    private readonly IObservableCollection<IMessageViewModel> messages;
    private readonly IObservableCollection<User> users;
    private readonly Dispatcher dispatcher;

    private RoomView view;
    private string message;

    public RoomViewModel(Room room)
    {
      this.room = room;
      this.dispatcher = Dispatcher.CurrentDispatcher;
      this.messages = new BindableCollection<IMessageViewModel>();
      this.users = new BindableCollection<User>();
    }

    public override string DisplayName
    {
      get { return room.Name; }
      set { }
    }

    public string Topic { get { return room.Topic; } }
    public IEnumerable<User> Users { get { return users; } }
    public IEnumerable<IMessageViewModel> Messages { get { return messages; } }

    public string Message
    {
      get { return message; }
      set
      {
        message = value;
        NotifyOfPropertyChange("Message");
        NotifyOfPropertyChange("CanSendMessage");
      }
    }

    public void SendMessage()
    {
      if (string.IsNullOrWhiteSpace(Message))
        return;

      var msg = Message;
      new Task(() => room.Say(msg)).Start();
      Message = string.Empty;
    }

    public void InsertNewLine()
    {
      view.Message.SelectedText = "\r\n";
      view.Message.SelectionStart = view.Message.SelectionStart + view.Message.SelectionLength;
      view.Message.SelectionLength = 0;
    }

    protected override void OnInitialize()
    {
      room.Join();

      users.AddRange(room.Users);

      var streamingContext = room.GetStreamingContext(AddMessage);
      var startStream = new Task(streamingContext.BeginStreaming);
      startStream.Start();
    }

    protected override void OnViewAttached(object view, object context)
    {
      this.view = (RoomView) view;
    }

    private void AddMessage(Message m)
    {
      IMessageViewModel msg;
      
      // This would be a great opportunity for refactoring
      // for polymorphism >.< There's a lot of copy/paste
      // between these different view models.
      switch (m.Type)
      {
        case MessageType.Text:
          msg = new MessageViewModel(m.User.Name, m.Body, m.User.ID == room.Site.GetMe().ID);
          break;
        case MessageType.Timestamp:
          msg = new TimeStampMessageViewModel(m.CreatedAt);
          break;
        case MessageType.Enter:
        case MessageType.Leave:
          msg = new RosterMessageViewModel(m.User.Name, m.Type);

          if (m.Type == MessageType.Enter)
          {
            if (!users.Any(u => u.ID == m.User.ID))
              users.Add(m.User);
          }
          else
          {
            users.RemoveRange(users.Where(u => u.ID == m.User.ID).ToList());
          }

          break;
        case MessageType.Paste:
          msg = new PasteMessageViewModel(m.User.Name, m.Body, m.User.ID == room.Site.GetMe().ID);
          break;
        default:
          return;
      }

      dispatcher.BeginInvoke((ThreadStart) (() => {
        messages.Add(msg);
        view.MessageScroller.ScrollToBottom();
      }));
    }
  }
}