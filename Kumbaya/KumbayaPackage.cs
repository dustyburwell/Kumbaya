using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace Kumbaya
{
  [PackageRegistration(UseManagedResourcesOnly = true)]
  [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
  [ProvideMenuResource("Menus.ctmenu", 1)]
  [ProvideToolWindow(typeof(MyToolWindow))]
  [Guid(GuidList.guidKumbayaPkgString)]
  public sealed class KumbayaPackage : Package
  {
    public KumbayaPackage()
    {
      Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
    }

    private void ShowToolWindow(object sender, EventArgs e)
    {
      ToolWindowPane window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
      if ((null == window) || (null == window.Frame))
      {
        throw new NotSupportedException(Resources.CanNotCreateWindow);
      }
      IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
      Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
    }

    protected override void Initialize()
    {
      Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
      base.Initialize();

      // Add our command handlers for menu (commands must exist in the .vsct file)
      OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
      if ( null != mcs )
      {
        // Create the command for the tool window
        CommandID toolwndCommandID = new CommandID(GuidList.guidKumbayaCmdSet, (int)PkgCmdIDList.Kumbaya);
        MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
        mcs.AddCommand( menuToolWin );
      }
    }
  }
}
