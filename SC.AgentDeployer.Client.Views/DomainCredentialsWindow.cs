using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using SC.AgentDeployer.Client.Helpers;

namespace SC.AgentDeployer.Client.Views;

public partial class DomainCredentialsWindow : Window, IComponentConnector
{
	public DomainCredentialsWindow()
	{
		InitializeComponent();
	}

	protected override void OnInitialized(EventArgs e)
	{
		if (base.DataContext is ICloseable closeable)
		{
			closeable.CloseRequested += delegate
			{
				Close();
			};
		}
		base.OnInitialized(e);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
