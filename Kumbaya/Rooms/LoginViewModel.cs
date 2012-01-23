using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using GrayHills.Matches;

namespace Kumbaya.Rooms
{
  public class LoginViewModel : Screen
  {
    private readonly RootViewModel rootViewModel;

    public LoginViewModel(RootViewModel rootViewModel)
    {
      this.rootViewModel = rootViewModel;
      CanLogin = true;
      WorkingText = "Login";
    }

    public string SiteName { get; set; }
    public string UserName { get; set; }
    public string Error { get; set; }
    public string WorkingText { get; set; }

    public bool CanLogin { get; private set; }

    public void Login()
    {
      if (string.IsNullOrWhiteSpace(SiteName))
      {
        Error = "Site cannot be empty";
        NotifyOfPropertyChange("Error");
        return;
      }

      WorkingText = "Signing in...";
      NotifyOfPropertyChange("WorkingText");
      CanLogin = false;
      NotifyOfPropertyChange("CanLogin");

      var loginTask =  new Task<Site>(() => {
        var site = new Site {Name = SiteName};
        var password = ((LoginView)GetView()).Password.Password;

        string apiToken = site.GetToken(new NetworkCredential(UserName, password));
        site.ApiToken = apiToken;
        site.Credentials = new NetworkCredential(apiToken, "X");

        return site;
      });

      loginTask.ContinueWith(t => rootViewModel.ActivateItem(new SiteViewModel(t.Result)),
        CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
      
      loginTask.ContinueWith(t => {
        var e = t.Exception.InnerException as WebException;
        WorkingText = "Login";
        NotifyOfPropertyChange("WorkingText");
        CanLogin = true;
        NotifyOfPropertyChange("CanLogin");
        
        if (e == null)
          return;

        switch (((HttpWebResponse)e.Response).StatusCode)
        {
          case HttpStatusCode.Unauthorized:
            Error = "Username or password was incorrect";
            break;
          case HttpStatusCode.NotFound:
            Error = "Site does not exist";
            break;
          default:
            Error = "There was an unknown error while authenticating. Try again.";
            break;
        }

        NotifyOfPropertyChange("Error");
      }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

      loginTask.Start();
    }
  }
}