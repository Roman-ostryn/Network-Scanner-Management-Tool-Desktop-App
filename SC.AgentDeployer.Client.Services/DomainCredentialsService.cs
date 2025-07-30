using System.Security;
using SC.AgentDeployer.Client.Helpers;

namespace SC.AgentDeployer.Client.Services;

public class DomainCredentialsService : SingletonServiceBase<DomainCredentialsService>
{
	private string domainName;

	private string domainUser;

	private SecureString domainPassword;

	public string DomainName
	{
		get
		{
			return domainName;
		}
		set
		{
			domainName = value;
			this.NotifyPropertyChanged((DomainCredentialsService _) => _.DomainName);
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
			this.NotifyPropertyChanged((DomainCredentialsService _) => _.DomainUser);
		}
	}

	public SecureString DomainPassword
	{
		get
		{
			return domainPassword;
		}
		set
		{
			domainPassword = value;
			this.NotifyPropertyChanged((DomainCredentialsService _) => _.DomainPassword);
		}
	}
}
