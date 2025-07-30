using SC.AgentDeployer.Client.Helpers;

namespace SC.AgentDeployer.Client.Models;

public class Machine : ObservableObject
{
	private bool isSelected;

	private string ip;

	private string status;

	private bool isMachineCapableAndClientNotInstalled;

	private MachineQueryState state;

	public bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			isSelected = value;
			this.NotifyPropertyChanged((Machine _) => _.IsSelected);
		}
	}

	public string Ip
	{
		get
		{
			return ip;
		}
		set
		{
			ip = value;
			this.NotifyPropertyChanged((Machine _) => _.Ip);
		}
	}

	public string Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
			this.NotifyPropertyChanged((Machine _) => _.Status);
		}
	}

	public bool IsMachineCapableAndClientNotInstalled
	{
		get
		{
			return isMachineCapableAndClientNotInstalled;
		}
		set
		{
			isMachineCapableAndClientNotInstalled = value;
			this.NotifyPropertyChanged((Machine _) => _.IsMachineCapableAndClientNotInstalled);
		}
	}

	public MachineQueryState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
			this.NotifyPropertyChanged((Machine _) => _.State);
		}
	}
}
