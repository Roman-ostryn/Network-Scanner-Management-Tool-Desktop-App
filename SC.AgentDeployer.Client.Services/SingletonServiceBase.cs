using System;
using SC.AgentDeployer.Client.Helpers;

namespace SC.AgentDeployer.Client.Services;

public abstract class SingletonServiceBase<T> : ObservableObject where T : class, new()
{
	private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());

	public static T Instance => lazy.Value;
}
