using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SC.AgentDeployer.Client.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("SC.AgentDeployer.Client.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string ClientDetailsNotLoaded => ResourceManager.GetString("ClientDetailsNotLoaded", resourceCulture);

	internal static string ClientStartStatusAccessDenied => ResourceManager.GetString("ClientStartStatusAccessDenied", resourceCulture);

	internal static string ClientStartStatusClientNotInstalledError => ResourceManager.GetString("ClientStartStatusClientNotInstalledError", resourceCulture);

	internal static string ClientStartStatusClientServiceDoesNotExist => ResourceManager.GetString("ClientStartStatusClientServiceDoesNotExist", resourceCulture);

	internal static string ClientStartStatusClientServiceOtherError => ResourceManager.GetString("ClientStartStatusClientServiceOtherError", resourceCulture);

	internal static string ClientStartStatusInvalidClientServiceHandle => ResourceManager.GetString("ClientStartStatusInvalidClientServiceHandle", resourceCulture);

	internal static string ClientStartStatusInvalidClientServiceName => ResourceManager.GetString("ClientStartStatusInvalidClientServiceName", resourceCulture);

	internal static string ClientStartStatusInvalidCredentials => ResourceManager.GetString("ClientStartStatusInvalidCredentials", resourceCulture);

	internal static string ClientStartStatusNoResponseToArp => ResourceManager.GetString("ClientStartStatusNoResponseToArp", resourceCulture);

	internal static string ClientStartStatusScmDoesNotExist => ResourceManager.GetString("ClientStartStatusScmDoesNotExist", resourceCulture);

	internal static string ClientStartStatusScmOtherError => ResourceManager.GetString("ClientStartStatusScmOtherError", resourceCulture);

	internal static string ClientStartStatusServiceContinuePending => ResourceManager.GetString("ClientStartStatusServiceContinuePending", resourceCulture);

	internal static string ClientStartStatusServiceNotStarted => ResourceManager.GetString("ClientStartStatusServiceNotStarted", resourceCulture);

	internal static string ClientStartStatusServicePausePending => ResourceManager.GetString("ClientStartStatusServicePausePending", resourceCulture);

	internal static string ClientStartStatusServiceRunning => ResourceManager.GetString("ClientStartStatusServiceRunning", resourceCulture);

	internal static string ClientStartStatusServiceStarted => ResourceManager.GetString("ClientStartStatusServiceStarted", resourceCulture);

	internal static string ClientStartStatusServiceStartPending => ResourceManager.GetString("ClientStartStatusServiceStartPending", resourceCulture);

	internal static string ClientStartStatusServiceStopPending => ResourceManager.GetString("ClientStartStatusServiceStopPending", resourceCulture);

	internal static string ClientStartStatusStartingService => ResourceManager.GetString("ClientStartStatusStartingService", resourceCulture);

	internal static string ClientStartStatusUnknownStatus => ResourceManager.GetString("ClientStartStatusUnknownStatus", resourceCulture);

	internal static string ClientStatusAccessDenied => ResourceManager.GetString("ClientStatusAccessDenied", resourceCulture);

	internal static string ClientStatusClientNotInstalledError => ResourceManager.GetString("ClientStatusClientNotInstalledError", resourceCulture);

	internal static string ClientStatusClientServiceDoesNotExist => ResourceManager.GetString("ClientStatusClientServiceDoesNotExist", resourceCulture);

	internal static string ClientStatusClientServiceOtherError => ResourceManager.GetString("ClientStatusClientServiceOtherError", resourceCulture);

	internal static string ClientStatusFailedOpenScm => ResourceManager.GetString("ClientStatusFailedOpenScm", resourceCulture);

	internal static string ClientStatusInvalidClientServiceHandle => ResourceManager.GetString("ClientStatusInvalidClientServiceHandle", resourceCulture);

	internal static string ClientStatusInvalidClientServiceName => ResourceManager.GetString("ClientStatusInvalidClientServiceName", resourceCulture);

	internal static string ClientStatusInvalidCredentials => ResourceManager.GetString("ClientStatusInvalidCredentials", resourceCulture);

	internal static string ClientStatusNoResponseToArp => ResourceManager.GetString("ClientStatusNoResponseToArp", resourceCulture);

	internal static string ClientStatusPossibleNonWindowsMachine => ResourceManager.GetString("ClientStatusPossibleNonWindowsMachine", resourceCulture);

	internal static string ClientStatusScmDoesNotExist => ResourceManager.GetString("ClientStatusScmDoesNotExist", resourceCulture);

	internal static string ClientStatusServiceContinuePending => ResourceManager.GetString("ClientStatusServiceContinuePending", resourceCulture);

	internal static string ClientStatusServicePaused => ResourceManager.GetString("ClientStatusServicePaused", resourceCulture);

	internal static string ClientStatusServicePausePending => ResourceManager.GetString("ClientStatusServicePausePending", resourceCulture);

	internal static string ClientStatusServiceRunning => ResourceManager.GetString("ClientStatusServiceRunning", resourceCulture);

	internal static string ClientStatusServiceStartPending => ResourceManager.GetString("ClientStatusServiceStartPending", resourceCulture);

	internal static string ClientStatusServiceStopped => ResourceManager.GetString("ClientStatusServiceStopped", resourceCulture);

	internal static string ClientStatusServiceStopPending => ResourceManager.GetString("ClientStatusServiceStopPending", resourceCulture);

	internal static string ClientStatusUnknownStatus => ResourceManager.GetString("ClientStatusUnknownStatus", resourceCulture);

	internal static string CredentialsUsingCurrentUserCredentials => ResourceManager.GetString("CredentialsUsingCurrentUserCredentials", resourceCulture);

	internal static string CredentialsUsingDomainCredentials => ResourceManager.GetString("CredentialsUsingDomainCredentials", resourceCulture);

	internal static string DefaultSubnetFilterApplied => ResourceManager.GetString("DefaultSubnetFilterApplied", resourceCulture);

	internal static string DeployClientMessageAccessDenied => ResourceManager.GetString("DeployClientMessageAccessDenied", resourceCulture);

	internal static string DeployClientMessageClientInstallFailure => ResourceManager.GetString("DeployClientMessageClientInstallFailure", resourceCulture);

	internal static string DeployClientMessageDuplicateServiceName => ResourceManager.GetString("DeployClientMessageDuplicateServiceName", resourceCulture);

	internal static string DeployClientMessageInvalidCredentials => ResourceManager.GetString("DeployClientMessageInvalidCredentials", resourceCulture);

	internal static string DeployClientMessageInvalidNetworkerServiceHandle => ResourceManager.GetString("DeployClientMessageInvalidNetworkerServiceHandle", resourceCulture);

	internal static string DeployClientMessageInvalidNetworkerServiceName => ResourceManager.GetString("DeployClientMessageInvalidNetworkerServiceName", resourceCulture);

	internal static string DeployClientMessageNetworkerServiceAlreadyExists => ResourceManager.GetString("DeployClientMessageNetworkerServiceAlreadyExists", resourceCulture);

	internal static string DeployClientMessageNetworkerServiceCircularDependency => ResourceManager.GetString("DeployClientMessageNetworkerServiceCircularDependency", resourceCulture);

	internal static string DeployClientMessageNetworkerServiceMarkedForDeletion => ResourceManager.GetString("DeployClientMessageNetworkerServiceMarkedForDeletion", resourceCulture);

	internal static string DeployClientMessageNetworkerServiceOtherError => ResourceManager.GetString("DeployClientMessageNetworkerServiceOtherError", resourceCulture);

	internal static string DeployClientMessageNoResponseToArp => ResourceManager.GetString("DeployClientMessageNoResponseToArp", resourceCulture);

	internal static string DeployClientMessagePossibleNonWindowsMachine => ResourceManager.GetString("DeployClientMessagePossibleNonWindowsMachine", resourceCulture);

	internal static string DeployClientMessageScmDoesNotExist => ResourceManager.GetString("DeployClientMessageScmDoesNotExist", resourceCulture);

	internal static string DeployClientMessageServiceContinuePending => ResourceManager.GetString("DeployClientMessageServiceContinuePending", resourceCulture);

	internal static string DeployClientMessageServicePaused => ResourceManager.GetString("DeployClientMessageServicePaused", resourceCulture);

	internal static string DeployClientMessageServicePausePending => ResourceManager.GetString("DeployClientMessageServicePausePending", resourceCulture);

	internal static string DeployClientMessageServiceRunning => ResourceManager.GetString("DeployClientMessageServiceRunning", resourceCulture);

	internal static string DeployClientMessageServiceStartPending => ResourceManager.GetString("DeployClientMessageServiceStartPending", resourceCulture);

	internal static string DeployClientMessageServiceStopped => ResourceManager.GetString("DeployClientMessageServiceStopped", resourceCulture);

	internal static string DeployClientMessageServiceStopPending => ResourceManager.GetString("DeployClientMessageServiceStopPending", resourceCulture);

	internal static string DeployClientMessageUnknownStatus => ResourceManager.GetString("DeployClientMessageUnknownStatus", resourceCulture);

	internal static string InvalidIpAddressRangeExMessage => ResourceManager.GetString("InvalidIpAddressRangeExMessage", resourceCulture);

	internal static string MsiInstallerFileName => ResourceManager.GetString("MsiInstallerFileName", resourceCulture);

	internal static string NetworkerServiceFileName => ResourceManager.GetString("NetworkerServiceFileName", resourceCulture);

	internal static string StatusAttemptingClientDeployment => ResourceManager.GetString("StatusAttemptingClientDeployment", resourceCulture);

	internal static string StatusAttemptingClientDeployments => ResourceManager.GetString("StatusAttemptingClientDeployments", resourceCulture);

	internal static string StatusCancelingOperation => ResourceManager.GetString("StatusCancelingOperation", resourceCulture);

	internal static string StatusCheckClientStatusCanceled => ResourceManager.GetString("StatusCheckClientStatusCanceled", resourceCulture);

	internal static string StatusCheckClientStatusOnMachineCanceled => ResourceManager.GetString("StatusCheckClientStatusOnMachineCanceled", resourceCulture);

	internal static string StatusCheckingClientStatus => ResourceManager.GetString("StatusCheckingClientStatus", resourceCulture);

	internal static string StatusCheckingClientStatuses => ResourceManager.GetString("StatusCheckingClientStatuses", resourceCulture);

	internal static string StatusClearAllComplete => ResourceManager.GetString("StatusClearAllComplete", resourceCulture);

	internal static string StatusDeployClientFinished => ResourceManager.GetString("StatusDeployClientFinished", resourceCulture);

	internal static string StatusDeployingClientToIp => ResourceManager.GetString("StatusDeployingClientToIp", resourceCulture);

	internal static string StatusDeploymentCanceled => ResourceManager.GetString("StatusDeploymentCanceled", resourceCulture);

	internal static string StatusDeploymentToMachineCanceled => ResourceManager.GetString("StatusDeploymentToMachineCanceled", resourceCulture);

	internal static string StatusDomainCredentialsCleared => ResourceManager.GetString("StatusDomainCredentialsCleared", resourceCulture);

	internal static string StatusDomainCredentialsSaved => ResourceManager.GetString("StatusDomainCredentialsSaved", resourceCulture);

	internal static string StatusFinishedProcessingPayload => ResourceManager.GetString("StatusFinishedProcessingPayload", resourceCulture);

	internal static string StatusLoadClientStatusFinished => ResourceManager.GetString("StatusLoadClientStatusFinished", resourceCulture);

	internal static string StatusLoadedFromArpTable => ResourceManager.GetString("StatusLoadedFromArpTable", resourceCulture);

	internal static string StatusLoadedFromSubnet => ResourceManager.GetString("StatusLoadedFromSubnet", resourceCulture);

	internal static string StatusLoadingClientStatusForIp => ResourceManager.GetString("StatusLoadingClientStatusForIp", resourceCulture);

	internal static string StatusProcessingPayload => ResourceManager.GetString("StatusProcessingPayload", resourceCulture);

	internal static string StatusSelectAllComplete => ResourceManager.GetString("StatusSelectAllComplete", resourceCulture);

	internal static string StatusSelectIpBeforeCheckStatus => ResourceManager.GetString("StatusSelectIpBeforeCheckStatus", resourceCulture);

	internal static string StatusSelectIpBeforeDeployClient => ResourceManager.GetString("StatusSelectIpBeforeDeployClient", resourceCulture);

	internal static string StatusSelectIpBeforeStartClient => ResourceManager.GetString("StatusSelectIpBeforeStartClient", resourceCulture);

	internal static string StatusStartClientsFinished => ResourceManager.GetString("StatusStartClientsFinished", resourceCulture);

	internal static string StatusStartingClientOnIp => ResourceManager.GetString("StatusStartingClientOnIp", resourceCulture);

	internal static string StatusStartingClients => ResourceManager.GetString("StatusStartingClients", resourceCulture);

	internal static string SubnetFilterApplied => ResourceManager.GetString("SubnetFilterApplied", resourceCulture);

	internal Resources()
	{
	}
}
