using System;
using System.Windows;
using SC.AgentDeployer.Client.Helpers;

namespace SC.AgentDeployer.Client.ViewModels;

public abstract class ViewModelBase : ObservableObject, ICloseable
{
	public event EventHandler CloseRequested;

	protected ViewModelBase()
	{
		Initialize();
		InitializeCommands();
	}

	protected abstract void Initialize();

	protected abstract void InitializeCommands();

	public virtual void OnClosing()
	{
	}

	protected void CloseWindow()
	{
		this.CloseRequested?.Invoke(this, EventArgs.Empty);
	}

	protected void MarshalCallToMainThread(Action marshaledProc)
	{
		Application.Current?.Dispatcher?.Invoke(marshaledProc, new object[0]);
	}
}
