using System.ComponentModel;

namespace SC.AgentDeployer.Client.Helpers;

public abstract class ObservableObject : INotifyPropertyChanged, IRaisePropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	public virtual void NotifyPropertyChanged(string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
