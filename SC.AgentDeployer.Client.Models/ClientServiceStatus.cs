namespace SC.AgentDeployer.Client.Models;

public enum ClientServiceStatus
{
	InvalidCredentials,
	ClientServiceOtherError,
	ScmDoesNotExist,
	PossibleNonWindowsMachine,
	InvalidClientServiceHandle,
	InvalidClientServiceName,
	ClientServiceDoesNotExist,
	ClientNotInstalledError,
	AccessDenied,
	NoResponseToArp,
	ServiceContinuePending,
	ServicePausePending,
	ServicePaused,
	ServiceRunning,
	ServiceStartPending,
	ServiceStopPending,
	ServiceStopped,
	UnknownStatus,
	ServiceStarted,
	ServiceNotStarted
}
