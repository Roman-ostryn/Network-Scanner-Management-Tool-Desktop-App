using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using SC.AgentDeployer.Client.ViewModels;

namespace SC.AgentDeployer.Client.Views;

public partial class MainWindow : Window, IComponentConnector
{
	public MainWindow()
	{
		InitializeComponent();
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (base.DataContext is MainViewModel mainViewModel)
		{
			mainViewModel.OnClosing();
		}
		base.OnClosing(e);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
