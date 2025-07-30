using System;

namespace SC.AgentDeployer.Client.Helpers;

public interface ICloseable
{
	event EventHandler CloseRequested;
}
