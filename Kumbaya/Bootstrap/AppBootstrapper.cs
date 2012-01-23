using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;

namespace Kumbaya.Bootstrap
{
  public class AppBootstrapper : Bootstrapper
  {
    private readonly object rootModel;
    private readonly RootControl control;

    public AppBootstrapper(object rootModel, RootControl control)
      : base(false)
    {
      this.rootModel = rootModel;
      this.control = control;
    }

    public void Start()
    {
      StartRuntime();
      OnStartup(this, null);
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
      var view = ViewLocator.LocateForModel(rootModel, null, null);
      ViewModelBinder.Bind(rootModel, view, null);

      var activator = rootModel as IActivate;
      if (activator != null)
        activator.Activate();

      control.Content = view;
    }

    protected override IEnumerable<Assembly> SelectAssemblies()
    {
      return new[]{ GetType().Assembly };
    }
  }
}