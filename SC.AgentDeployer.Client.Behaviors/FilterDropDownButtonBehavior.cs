using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace SC.AgentDeployer.Client.Behaviors;

public class FilterDropDownButtonBehavior : Behavior<Button>
{
	private long attachedCount;

	private bool isContextMenuOpen;

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), handledEventsToo: true);
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
	}

	private void AssociatedObject_Click(object sender, RoutedEventArgs e)
	{
		if (sender is Button { ContextMenu: not null } button && !isContextMenuOpen)
		{
			button.ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed), handledEventsToo: true);
			Interlocked.Increment(ref attachedCount);
			button.ContextMenu.PlacementTarget = button;
			button.ContextMenu.Placement = PlacementMode.Bottom;
			button.ContextMenu.IsOpen = true;
			isContextMenuOpen = true;
		}
	}

	private void ContextMenu_Closed(object sender, RoutedEventArgs e)
	{
		isContextMenuOpen = false;
		if (sender is ContextMenu contextMenu)
		{
			contextMenu.RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed));
			Interlocked.Decrement(ref attachedCount);
		}
	}
}
