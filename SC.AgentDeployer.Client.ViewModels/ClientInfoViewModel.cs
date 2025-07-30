using System;
using System.Windows.Input;
using SC.AgentDeployer.Client.Commands;
using SC.AgentDeployer.Client.Helpers;
using SC.AgentDeployer.Client.Services;
using ScreenConnect;

namespace SC.AgentDeployer.Client.ViewModels;

public class ClientInfoViewModel : ViewModelBase
{
	private string clientServiceName;

	private string clientServiceVersion;

	private string clientServiceRelayUrl;

	private string customProperty1;

	private string customProperty2;

	private string customProperty3;

	private string customProperty4;

	private ClientLaunchParameters clientLaunchParameters;

	public string ClientServiceName => clientServiceName;

	public string ClientServiceVersion => clientServiceVersion;

	public string ClientServiceRelayUrl => clientServiceRelayUrl;

	public string CustomProperty1
	{
		get
		{
			return customProperty1;
		}
		set
		{
			customProperty1 = value;
			this.NotifyPropertyChanged((ClientInfoViewModel _) => _.CustomProperty1);
		}
	}

	public string CustomProperty2
	{
		get
		{
			return customProperty2;
		}
		set
		{
			customProperty2 = value;
			this.NotifyPropertyChanged((ClientInfoViewModel _) => _.CustomProperty2);
		}
	}

	public string CustomProperty3
	{
		get
		{
			return customProperty3;
		}
		set
		{
			customProperty3 = value;
			this.NotifyPropertyChanged((ClientInfoViewModel _) => _.CustomProperty3);
		}
	}

	public string CustomProperty4
	{
		get
		{
			return customProperty4;
		}
		set
		{
			customProperty4 = value;
			this.NotifyPropertyChanged((ClientInfoViewModel _) => _.CustomProperty4);
		}
	}

	public ICommand CloseCommand { get; private set; }

	public ICommand SaveCommand { get; private set; }

	protected override void Initialize()
	{
		clientServiceName = SingletonServiceBase<ClientInfoService>.Instance.ClientServiceName;
		clientServiceVersion = SingletonServiceBase<ClientInfoService>.Instance.ClientServiceVersion;
		clientServiceRelayUrl = SingletonServiceBase<ClientInfoService>.Instance.ClientServiceRelayUrl;
		clientLaunchParameters = SingletonServiceBase<ClientInfoService>.Instance.ClientLaunchParameters;
		ClientLaunchParameters obj = clientLaunchParameters;
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			string[] customPropertyValueCallbackFormats = obj.CustomPropertyValueCallbackFormats;
			obj2 = ((customPropertyValueCallbackFormats != null) ? customPropertyValueCallbackFormats[0] : null);
		}
		customProperty1 = (string)obj2;
		ClientLaunchParameters obj3 = clientLaunchParameters;
		object obj4;
		if (obj3 == null)
		{
			obj4 = null;
		}
		else
		{
			string[] customPropertyValueCallbackFormats2 = obj3.CustomPropertyValueCallbackFormats;
			obj4 = ((customPropertyValueCallbackFormats2 != null) ? customPropertyValueCallbackFormats2[1] : null);
		}
		customProperty2 = (string)obj4;
		ClientLaunchParameters obj5 = clientLaunchParameters;
		object obj6;
		if (obj5 == null)
		{
			obj6 = null;
		}
		else
		{
			string[] customPropertyValueCallbackFormats3 = obj5.CustomPropertyValueCallbackFormats;
			obj6 = ((customPropertyValueCallbackFormats3 != null) ? customPropertyValueCallbackFormats3[2] : null);
		}
		customProperty3 = (string)obj6;
		ClientLaunchParameters obj7 = clientLaunchParameters;
		object obj8;
		if (obj7 == null)
		{
			obj8 = null;
		}
		else
		{
			string[] customPropertyValueCallbackFormats4 = obj7.CustomPropertyValueCallbackFormats;
			obj8 = ((customPropertyValueCallbackFormats4 != null) ? customPropertyValueCallbackFormats4[3] : null);
		}
		customProperty4 = (string)obj8;
	}

	protected override void InitializeCommands()
	{
		CloseCommand = new RelayCommand(delegate
		{
			CloseWindow();
		});
		SaveCommand = new RelayCommand(delegate
		{
			SaveClientLaunchParameters();
		});
	}

	private void SaveClientLaunchParameters()
	{
		if (clientLaunchParameters == null)
		{
			throw new InvalidOperationException("ClientLaunchParameters can't be null");
		}
		if (clientLaunchParameters.CustomPropertyValueCallbackFormats == null)
		{
			clientLaunchParameters.CustomPropertyValueCallbackFormats = new string[8];
		}
		clientLaunchParameters.CustomPropertyValueCallbackFormats[0] = CustomProperty1 ?? string.Empty;
		clientLaunchParameters.CustomPropertyValueCallbackFormats[1] = CustomProperty2 ?? string.Empty;
		clientLaunchParameters.CustomPropertyValueCallbackFormats[2] = CustomProperty3 ?? string.Empty;
		clientLaunchParameters.CustomPropertyValueCallbackFormats[3] = CustomProperty4 ?? string.Empty;
		SingletonServiceBase<ClientInfoService>.Instance.ClientLaunchParameters = clientLaunchParameters;
		CloseWindow();
	}
}
