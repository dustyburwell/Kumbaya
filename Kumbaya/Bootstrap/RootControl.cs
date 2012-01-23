using System;
using System.Windows.Controls;

namespace Kumbaya.Bootstrap
{
  public class RootControl : ContentPresenter
  {
    private readonly AppBootstrapper bootstrapper;

    public RootControl(object rootModel)
    {
      bootstrapper = new AppBootstrapper(rootModel, this);
    }

    protected override void OnInitialized(EventArgs e)
    {
      bootstrapper.Start();
    }
  }
}
