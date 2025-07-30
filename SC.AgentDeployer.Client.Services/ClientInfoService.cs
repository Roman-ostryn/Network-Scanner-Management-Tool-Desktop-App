using SC.AgentDeployer.Client.Helpers;
using ScreenConnect;

namespace SC.AgentDeployer.Client.Services;

public class ClientInfoService : SingletonServiceBase<ClientInfoService>
{
	private string clientServiceName;

	private string clientServiceVersion;

	private string clientServiceRelayUrl;

	private ClientLaunchParameters clientLaunchParameters;

	public string ClientServiceName
	{
		get
		{
			return clientServiceName;
		}
		set
		{
			clientServiceName = value;
			this.NotifyPropertyChanged((ClientInfoService _) => _.ClientServiceName);
		}
	}

	public string ClientServiceVersion
	{
		get
		{
			return clientServiceVersion;
		}
		set
		{
			clientServiceVersion = value;
			this.NotifyPropertyChanged((ClientInfoService _) => _.ClientServiceVersion);
		}
	}

	public string ClientServiceRelayUrl
	{
		get
		{
			return clientServiceRelayUrl;
		}
		set
		{
			clientServiceRelayUrl = value;
			this.NotifyPropertyChanged((ClientInfoService _) => _.ClientServiceRelayUrl);
		}
	}

	public ClientLaunchParameters ClientLaunchParameters
	{
		get
		{
			return clientLaunchParameters;
		}
		set
		{
			clientLaunchParameters = value;
			this.NotifyPropertyChanged((ClientInfoService _) => _.ClientLaunchParameters);
		}
	}
}
