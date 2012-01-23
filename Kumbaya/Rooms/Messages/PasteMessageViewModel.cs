using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Kumbaya.Rooms.Messages
{
  internal class PasteMessageViewModel : MessageViewModel
  {
    private readonly IEnumerable<string> lines;
    
    public PasteMessageViewModel(string name, string body, bool isMe)
      : base(name, body, isMe)
    {
      lines = Body.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
    }

    public bool IsTruncated { get { return lines.Count() > 14; } }
    public int NumberOfTruncatedLines { get { return lines.Count() - 14; } }
    public string TruncatedBody { get { return string.Join("\r\n", lines.Take(14)); } }
    public Visibility TruncatedStuffVisibility { 
      get { return IsTruncated ? Visibility.Visible : Visibility.Collapsed; }
    }

    public void ViewPaste()
    {
      // This is lame
      MessageBox.Show(Body, "Paste Text");
    }
  }
}