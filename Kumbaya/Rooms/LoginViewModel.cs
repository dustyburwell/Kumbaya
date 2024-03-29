﻿using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using GrayHills.Matches;

namespace Kumbaya.Rooms
{
  public class LoginViewModel : Screen
  {
    private readonly RootViewModel rootViewModel;

    private string workingText;
    private string error;
    private bool canLogin;

    public LoginViewModel(RootViewModel rootViewModel)
    {
      this.rootViewModel = rootViewModel;
      CanLogin = true;
      WorkingText = "Login";
    }

    public string SiteName { get; set; }
    public string UserName { get; set; }

    public string Error 
    { 
      get { return error; }
      private set 
      {
        error = value;
        NotifyOfPropertyChange("Error");
      }
    }

    public string WorkingText { 
      get { return workingText; } 
      private set 
      {
        workingText = value;
        NotifyOfPropertyChange("WorkingText");
      }
    }

    public bool CanLogin { 
      get { return canLogin; }
      private set
      {
        canLogin = value;
        NotifyOfPropertyChange("CanLogin");
      }
    }

    public void Login()
    {
      if (string.IsNullOrWhiteSpace(SiteName))
      {
        Error = "Site cannot be empty";
        return;
      }

      WorkingText = "Signing in...";
      CanLogin = false;

      // This task stuff turns out to be very
      // untidy. I must find a cleaner way to
      // handle this.
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
        CanLogin = true;
        
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

      }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

      loginTask.Start();
    }
  }
}