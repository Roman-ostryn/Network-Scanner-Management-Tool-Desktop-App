using System;
using System.Windows.Input;

namespace SC.AgentDeployer.Client.Commands;

public class RelayCommand : ICommand
{
	private readonly Action<object> execute;

	private readonly Predicate<object> canExecute;

	public event EventHandler CanExecuteChanged
	{
		add
		{
			CommandManager.RequerySuggested += value;
		}
		remove
		{
			CommandManager.RequerySuggested -= value;
		}
	}

	public RelayCommand(Action<object> execute)
		: this(execute, null)
	{
	}

	public RelayCommand(Action<object> execute, Predicate<object> canExecute)
	{
		if (execute == null)
		{
			throw new ArgumentNullException("execute");
		}
		this.execute = execute;
		if (canExecute == null)
		{
			this.canExecute = (object _) => true;
		}
		else
		{
			this.canExecute = canExecute;
		}
	}

	public bool CanExecute(object parameter)
	{
		return canExecute(parameter);
	}

	public void Execute(object parameter)
	{
		if (canExecute(parameter))
		{
			execute(parameter);
		}
	}
}
public class RelayCommand<T> : ICommand
{
	private readonly Action<T> execute;

	private readonly Predicate<T> canExecute;

	public event EventHandler CanExecuteChanged
	{
		add
		{
			CommandManager.RequerySuggested += value;
		}
		remove
		{
			CommandManager.RequerySuggested -= value;
		}
	}

	public RelayCommand(Action<T> execute)
		: this(execute, (Predicate<T>)null)
	{
	}

	public RelayCommand(Action<T> execute, Predicate<T> canExecute)
	{
		if (execute == null)
		{
			throw new ArgumentNullException("execute");
		}
		this.execute = execute;
		if (canExecute == null)
		{
			this.canExecute = (T _) => true;
		}
		else
		{
			this.canExecute = canExecute;
		}
	}

	public bool CanExecute(object parameter)
	{
		return canExecute((T)parameter);
	}

	public void Execute(object parameter)
	{
		if (canExecute((T)parameter))
		{
			execute((T)parameter);
		}
	}
}
