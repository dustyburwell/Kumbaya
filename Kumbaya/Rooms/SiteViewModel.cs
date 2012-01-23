using Caliburn.Micro;
using GrayHills.Matches;

namespace Kumbaya.Rooms
{
  public class SiteViewModel : Conductor<Screen>.Collection.OneActive
  {
    private readonly Site site;

    public SiteViewModel(Site site)
    {
      this.site = site;
    }

    protected override void OnActivate()
    {
      Items.Add(new LobbyViewModel(this, site));
    }
  }
}