using System.Windows;
using System.Windows.Controls;

namespace SC.AgentDeployer.Client.Helpers;

[TemplatePart(Name = "Border", Type = typeof(Border))]
public class LoadingIndicator : Control
{
	public static readonly DependencyProperty SpeedRatioProperty = DependencyProperty.Register("SpeedRatio", typeof(double), typeof(LoadingIndicator), new PropertyMetadata(1.0, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
	{
		LoadingIndicator loadingIndicator = (LoadingIndicator)o;
		if (loadingIndicator.PART_Border == null || !loadingIndicator.IsActive)
		{
			return;
		}
		foreach (VisualStateGroup visualStateGroup2 in VisualStateManager.GetVisualStateGroups(loadingIndicator.PART_Border))
		{
			if (visualStateGroup2.Name == "ActiveStates")
			{
				foreach (VisualState state in visualStateGroup2.States)
				{
					if (state.Name == "Active")
					{
						state.Storyboard.SetSpeedRatio(loadingIndicator.PART_Border, (double)e.NewValue);
					}
				}
			}
		}
	}));

	public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(true, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
	{
		LoadingIndicator loadingIndicator = (LoadingIndicator)o;
		if (loadingIndicator.PART_Border != null)
		{
			if ((bool)e.NewValue)
			{
				VisualStateManager.GoToElementState(loadingIndicator.PART_Border, "Active", useTransitions: false);
				loadingIndicator.PART_Border.Visibility = Visibility.Visible;
				{
					foreach (VisualStateGroup visualStateGroup3 in VisualStateManager.GetVisualStateGroups(loadingIndicator.PART_Border))
					{
						if (visualStateGroup3.Name == "ActiveStates")
						{
							foreach (VisualState state2 in visualStateGroup3.States)
							{
								if (state2.Name == "Active")
								{
									state2.Storyboard.SetSpeedRatio(loadingIndicator.PART_Border, loadingIndicator.SpeedRatio);
								}
							}
						}
					}
					return;
				}
			}
			VisualStateManager.GoToElementState(loadingIndicator.PART_Border, "Inactive", useTransitions: false);
			loadingIndicator.PART_Border.Visibility = Visibility.Collapsed;
		}
	}));

	protected Border PART_Border;

	public double SpeedRatio
	{
		get
		{
			return (double)GetValue(SpeedRatioProperty);
		}
		set
		{
			SetValue(SpeedRatioProperty, value);
		}
	}

	public bool IsActive
	{
		get
		{
			return (bool)GetValue(IsActiveProperty);
		}
		set
		{
			SetValue(IsActiveProperty, value);
		}
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		PART_Border = (Border)GetTemplateChild("PART_Border");
		if (PART_Border == null)
		{
			return;
		}
		VisualStateManager.GoToElementState(PART_Border, IsActive ? "Active" : "Inactive", useTransitions: false);
		foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(PART_Border))
		{
			if (!(visualStateGroup.Name == "ActiveStates"))
			{
				continue;
			}
			foreach (VisualState state in visualStateGroup.States)
			{
				if (state.Name == "Active")
				{
					state.Storyboard.SetSpeedRatio(PART_Border, SpeedRatio);
				}
			}
		}
		PART_Border.Visibility = ((!IsActive) ? Visibility.Collapsed : Visibility.Visible);
	}
}
