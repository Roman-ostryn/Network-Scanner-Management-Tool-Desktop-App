namespace SC.AgentDeployer.Client.Models;

public enum DeployClientResult
{
	ScmDoesNotExist,
	PossibleNonWindowsMachine,
	InvalidNetworkerServiceName,
	NetworkerServiceOtherError,
	NetworkerServiceCircularDependency,
	NetworkerServiceMarkedForDeletion,
	NetworkerServiceAlreadyExists,
	DuplicateServiceName,
	ClientInstallFailure,
	AccessDenied,
	InvalidCredentials,
	NoResponseToArp,
	ServiceContinuePending,
	ServicePausePending,
	ServicePaused,
	ServiceRunning,
	ServiceStartPending,
	ServiceStopPending,
	ServiceStopped,
	UnknownStatus
}
