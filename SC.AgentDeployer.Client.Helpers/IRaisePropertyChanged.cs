namespace SC.AgentDeployer.Client.Helpers;

public interface IRaisePropertyChanged
{
	void NotifyPropertyChanged(string propertyName = null);
}
