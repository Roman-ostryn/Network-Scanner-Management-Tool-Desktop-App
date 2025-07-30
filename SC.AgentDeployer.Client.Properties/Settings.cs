using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SC.AgentDeployer.Client.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Processing networker payload...")]
	public string StatusProcessingPayload
	{
		get
		{
			return (string)this["StatusProcessingPayload"];
		}
		set
		{
			this["StatusProcessingPayload"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Finished processing networker payload")]
	public string StatusFinishedProcessingPayload
	{
		get
		{
			return (string)this["StatusFinishedProcessingPayload"];
		}
		set
		{
			this["StatusFinishedProcessingPayload"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Loaded subnet addresses")]
	public string StatusLoadedFromSubnet
	{
		get
		{
			return (string)this["StatusLoadedFromSubnet"];
		}
		set
		{
			this["StatusLoadedFromSubnet"] = value;
		}
	}

	[DefaultSettingValue("Loaded addresses from arp table")]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public string StatusLoadedFromArpTable
	{
		get
		{
			return (string)this["StatusLoadedFromArpTable"];
		}
		set
		{
			this["StatusLoadedFromArpTable"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("Finished loading client statuses")]
	[UserScopedSetting]
	public string StatusLoadStatusFinished
	{
		get
		{
			return (string)this["StatusLoadStatusFinished"];
		}
		set
		{
			this["StatusLoadStatusFinished"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("Finished attempting client deployment")]
	public string StatusDeployClientFinished
	{
		get
		{
			return (string)this["StatusDeployClientFinished"];
		}
		set
		{
			this["StatusDeployClientFinished"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("Select all complete")]
	[UserScopedSetting]
	public string StatusSelectAllComplete
	{
		get
		{
			return (string)this["StatusSelectAllComplete"];
		}
		set
		{
			this["StatusSelectAllComplete"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Clear selection complete")]
	public string StatusClearAllComplete
	{
		get
		{
			return (string)this["StatusClearAllComplete"];
		}
		set
		{
			this["StatusClearAllComplete"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("SC.AgentDeployer.Installer.msi")]
	public string MsiInstallerFileName
	{
		get
		{
			return (string)this["MsiInstallerFileName"];
		}
		set
		{
			this["MsiInstallerFileName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("SC.AgentDeployer.Installer.exe")]
	public string NetworkerServiceFileName
	{
		get
		{
			return (string)this["NetworkerServiceFileName"];
		}
		set
		{
			this["NetworkerServiceFileName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Loading status for {0}...")]
	public string StatusLoadingStatusForIp
	{
		get
		{
			return (string)this["StatusLoadingStatusForIp"];
		}
		set
		{
			this["StatusLoadingStatusForIp"] = value;
		}
	}

	[DefaultSettingValue("Attempting client deployment to  {0}...")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public string StatusDeployingToIp
	{
		get
		{
			return (string)this["StatusDeployingToIp"];
		}
		set
		{
			this["StatusDeployingToIp"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Please enter an admin username and password")]
	public string StatusEnterValidCredentials
	{
		get
		{
			return (string)this["StatusEnterValidCredentials"];
		}
		set
		{
			this["StatusEnterValidCredentials"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Please select at least one IP address before checking status")]
	public string StatusSelectIpBeforeCheckStatus
	{
		get
		{
			return (string)this["StatusSelectIpBeforeCheckStatus"];
		}
		set
		{
			this["StatusSelectIpBeforeCheckStatus"] = value;
		}
	}

	[UserScopedSetting]
	[DefaultSettingValue("Please select at least one IP address before deploying client")]
	[DebuggerNonUserCode]
	public string StatusSelectIpBeforeDeployClient
	{
		get
		{
			return (string)this["StatusSelectIpBeforeDeployClient"];
		}
		set
		{
			this["StatusSelectIpBeforeDeployClient"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("Loading client information...")]
	[UserScopedSetting]
	public string LoadingClientInfo
	{
		get
		{
			return (string)this["LoadingClientInfo"];
		}
		set
		{
			this["LoadingClientInfo"] = value;
		}
	}

	[DefaultSettingValue("Service installation failed")]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public string ServiceInstallationFailed
	{
		get
		{
			return (string)this["ServiceInstallationFailed"];
		}
		set
		{
			this["ServiceInstallationFailed"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("Failed to open SCM on remote machine")]
	[UserScopedSetting]
	public string FailedOpenScm
	{
		get
		{
			return (string)this["FailedOpenScm"];
		}
		set
		{
			this["FailedOpenScm"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Client installed (SCM and client service handles opened and closed)")]
	public string ClientInstalledScmAndClientOpenAndClose
	{
		get
		{
			return (string)this["ClientInstalledScmAndClientOpenAndClose"];
		}
		set
		{
			this["ClientInstalledScmAndClientOpenAndClose"] = value;
		}
	}

	[DefaultSettingValue("Client installed (Client service handle opened and closed. SCM opened, but failed to close)")]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public string ClientInstalledScmOpenNotCloseClientOpenAndClose
	{
		get
		{
			return (string)this["ClientInstalledScmOpenNotCloseClientOpenAndClose"];
		}
		set
		{
			this["ClientInstalledScmOpenNotCloseClientOpenAndClose"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Client installed (Client service handle failed to close. SCM handle opened and closed)")]
	public string ClientInstalledScmOpenAndCloseClientOpenNotClose
	{
		get
		{
			return (string)this["ClientInstalledScmOpenAndCloseClientOpenNotClose"];
		}
		set
		{
			this["ClientInstalledScmOpenAndCloseClientOpenNotClose"] = value;
		}
	}

	[DefaultSettingValue("Client installed (Client service and SCM handles failed to close)")]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public string ClientInstalledScmAndClientOpenAndNotClose
	{
		get
		{
			return (string)this["ClientInstalledScmAndClientOpenAndNotClose"];
		}
		set
		{
			this["ClientInstalledScmAndClientOpenAndNotClose"] = value;
		}
	}

	[DefaultSettingValue("Client not installed (SCM handle opened and closed)")]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public string ClientNotInstalledScmOpenAndClose
	{
		get
		{
			return (string)this["ClientNotInstalledScmOpenAndClose"];
		}
		set
		{
			this["ClientNotInstalledScmOpenAndClose"] = value;
		}
	}

	[DefaultSettingValue("Client not installed (SCM handle opened, but failed to close)")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public string ClientNotInstalledScmOpenNotClose
	{
		get
		{
			return (string)this["ClientNotInstalledScmOpenNotClose"];
		}
		set
		{
			this["ClientNotInstalledScmOpenNotClose"] = value;
		}
	}
}
