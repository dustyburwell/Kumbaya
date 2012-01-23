using System.Runtime.InteropServices;
using Kumbaya.Bootstrap;
using Kumbaya.Rooms;
using Microsoft.VisualStudio.Shell;

namespace Kumbaya
{
  [Guid("14004daf-08a2-4a31-9f98-676c07208f79")]
  public class MyToolWindow : ToolWindowPane
  {
    public MyToolWindow() :
      base(null)
    {
      this.Caption = Resources.ToolWindowTitle;
      this.BitmapResourceID = 201;
      this.BitmapIndex = 0;

      base.Content = new RootControl(new RootViewModel());
    }
  }
}
