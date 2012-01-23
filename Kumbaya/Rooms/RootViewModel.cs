using Caliburn.Micro;

namespace Kumbaya.Rooms
{
  public class RootViewModel : Conductor<Screen>
  {
    protected override void OnActivate()
    {
      ActivateItem(new LoginViewModel(this));
    }
  }
}
