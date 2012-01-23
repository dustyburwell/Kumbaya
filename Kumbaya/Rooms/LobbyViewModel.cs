using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GrayHills.Matches;

namespace Kumbaya.Rooms
{
  public class LobbyViewModel : Screen
  {
    private readonly SiteViewModel siteViewModel;
    private readonly Site site;

    public LobbyViewModel(SiteViewModel siteViewModel, Site site)
    {
      this.siteViewModel = siteViewModel;
      this.site = site;
    }

    public override string DisplayName
    {
      get { return "Lobby"; }
      set { }
    }
    public IEnumerable<RoomViewModel> Rooms { get; private set; }

    protected override void OnActivate()
    {
      Rooms = site.GetRooms().Select(r => new RoomViewModel(r));
    }

    public void Join(RoomViewModel room)
    {
      siteViewModel.ActivateItem(room);
    }
  }
}