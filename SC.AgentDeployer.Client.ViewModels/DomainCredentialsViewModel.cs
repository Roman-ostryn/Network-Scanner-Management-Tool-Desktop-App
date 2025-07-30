using System;
using System.Windows.Controls;
using System.Windows.Input;
using SC.AgentDeployer.Client.Commands;
using SC.AgentDeployer.Client.Helpers;
using SC.AgentDeployer.Client.Services;

namespace SC.AgentDeployer.Client.ViewModels;

public class DomainCredentialsViewModel : ViewModelBase
{
	private string domainName;

	private string domainUser;

	private string currentUser;

	private string statusMessage;

	public string DomainName
	{
		get
		{
			return domainName;
		}
		set
		{
			domainName = value;
			this.NotifyPropertyChanged((DomainCredentialsViewModel _) => _.DomainName);
			UpdateCurrentUser();
		}
	}

	public string DomainUser
	{
		get
		{
			return domainUser;
		}
		set
		{
			domainUser = value;
			this.NotifyPropertyChanged((DomainCredentialsViewModel _) => _.DomainUser);
			UpdateCurrentUser();
		}
	}

	public string CurrentUser
	{
		get
		{
			return currentUser;
		}
		set
		{
			currentUser = value;
			this.NotifyPropertyChanged((DomainCredentialsViewModel _) => _.CurrentUser);
		}
	}

	public string StatusMessage
	{
		get
		{
			return statusMessage;
		}
		set
		{
			statusMessage = value;
			this.NotifyPropertyChanged((DomainCredentialsViewModel _) => _.StatusMessage);
		}
	}

	public ICommand SaveCredentialsCommand { get; private set; }

	public ICommand CloseCommand { get; private set; }

	protected override void Initialize()
	{
		if (!string.IsNullOrEmpty(SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser) || !string.IsNullOrEmpty(SingletonServiceBase<DomainCredentialsService>.Instance.DomainName))
		{
			domainName = SingletonServiceBase<DomainCredentialsService>.Instance.DomainName;
			domainUser = SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser;
		}
		UpdateCurrentUser();
	}

	protected override void InitializeCommands()
	{
		SaveCredentialsCommand = new RelayCommand<PasswordBox>(delegate(PasswordBox _)
		{
			SaveCredentials(_);
		});
		CloseCommand = new RelayCommand(delegate
		{
			CloseWindow();
		});
	}

	private void SaveCredentials(PasswordBox passwordBox)
	{
		if (!string.IsNullOrEmpty(DomainUser) && passwordBox != null)
		{
			SingletonServiceBase<DomainCredentialsService>.Instance.DomainName = DomainName;
			SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser = DomainUser;
			SingletonServiceBase<DomainCredentialsService>.Instance.DomainPassword = passwordBox.SecurePassword;
			CloseWindow();
		}
		else
		{
			StatusMessage = "Domain user required";
		}
	}

	private void UpdateCurrentUser()
	{
		if (!string.IsNullOrEmpty(DomainUser))
		{
			CurrentUser = ((!string.IsNullOrEmpty(DomainName)) ? (DomainName + "\\" + DomainUser) : DomainUser);
		}
		else
		{
			CurrentUser = ((!string.IsNullOrEmpty(Environment.UserDomainName)) ? (Environment.UserDomainName + "\\" + Environment.UserName) : Environment.UserName);
		}
	}
}
