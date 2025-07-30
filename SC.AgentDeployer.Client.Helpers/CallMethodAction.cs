using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;

namespace SC.AgentDeployer.Client.Helpers;

public class CallMethodAction : TriggerAction<DependencyObject>
{
	private class MethodDescriptor
	{
		public MethodInfo MethodInfo { get; private set; }

		public bool HasParameters => Parameters.Length != 0;

		public int ParameterCount => Parameters.Length;

		public ParameterInfo[] Parameters { get; private set; }

		public Type SecondParameterType
		{
			get
			{
				if (Parameters.Length < 2)
				{
					return null;
				}
				return Parameters[1].ParameterType;
			}
		}

		public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParams)
		{
			MethodInfo = methodInfo;
			Parameters = methodParams;
		}
	}

	private List<MethodDescriptor> methodDescriptors;

	public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(CallMethodAction), new PropertyMetadata(OnTargetObjectChanged));

	public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(CallMethodAction), new PropertyMetadata(OnMethodNameChanged));

	private object Target => TargetObject ?? base.AssociatedObject;

	public object TargetObject
	{
		get
		{
			return ((DependencyObject)(object)this).GetValue(TargetObjectProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(TargetObjectProperty, value);
		}
	}

	public string MethodName
	{
		get
		{
			return (string)((DependencyObject)(object)this).GetValue(MethodNameProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(MethodNameProperty, (object)value);
		}
	}

	public CallMethodAction()
	{
		methodDescriptors = new List<MethodDescriptor>();
	}

	protected override void Invoke(object parameter)
	{
		if (base.AssociatedObject == null)
		{
			return;
		}
		MethodDescriptor methodDescriptor = FindBestMethod(parameter);
		if (methodDescriptor != null)
		{
			ParameterInfo[] parameters = methodDescriptor.Parameters;
			if (parameters.Length == 0)
			{
				methodDescriptor.MethodInfo.Invoke(Target, null);
			}
			else if (parameters.Length == 2 && base.AssociatedObject != null && parameter != null && parameters[0].ParameterType.IsAssignableFrom(base.AssociatedObject.GetType()) && parameters[1].ParameterType.IsAssignableFrom(parameter.GetType()))
			{
				methodDescriptor.MethodInfo.Invoke(Target, new object[2] { base.AssociatedObject, parameter });
			}
		}
		else if (TargetObject != null)
		{
			throw new ArgumentException("TargetObject");
		}
	}

	protected override void OnAttached()
	{
		((TriggerAction)this).OnAttached();
		UpdateMethodInfo();
	}

	protected override void OnDetaching()
	{
		methodDescriptors.Clear();
		((TriggerAction)this).OnDetaching();
	}

	private static bool AreMethodParamsValid(ParameterInfo[] methodParams)
	{
		if (methodParams.Length == 2)
		{
			if (methodParams[0].ParameterType != typeof(object) || !typeof(EventArgs).IsAssignableFrom(methodParams[1].ParameterType))
			{
				return false;
			}
		}
		else if (methodParams.Length != 0)
		{
			return false;
		}
		return true;
	}

	private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		((CallMethodAction)(object)sender).UpdateMethodInfo();
	}

	private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		((CallMethodAction)(object)sender).UpdateMethodInfo();
	}

	private MethodDescriptor FindBestMethod(object parameter)
	{
		if (parameter != null)
		{
			parameter.GetType();
		}
		return methodDescriptors.FirstOrDefault(delegate(MethodDescriptor methodDescriptor)
		{
			if (!methodDescriptor.HasParameters)
			{
				return true;
			}
			return parameter != null && methodDescriptor.SecondParameterType.IsAssignableFrom(parameter.GetType());
		});
	}

	private void UpdateMethodInfo()
	{
		methodDescriptors.Clear();
		if (Target == null || string.IsNullOrEmpty(MethodName))
		{
			return;
		}
		MethodInfo[] methods = Target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
		foreach (MethodInfo methodInfo in methods)
		{
			if (IsMethodValid(methodInfo))
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (AreMethodParamsValid(parameters))
				{
					methodDescriptors.Add(new MethodDescriptor(methodInfo, parameters));
				}
			}
		}
		methodDescriptors = methodDescriptors.OrderByDescending(delegate(MethodDescriptor methodDescriptor)
		{
			int num = 0;
			if (methodDescriptor.HasParameters)
			{
				Type type = methodDescriptor.SecondParameterType;
				while (type != typeof(EventArgs))
				{
					num++;
					type = type.BaseType;
				}
			}
			return methodDescriptor.ParameterCount + num;
		}).ToList();
	}

	private bool IsMethodValid(MethodInfo method)
	{
		if (string.Equals(method.Name, MethodName, StringComparison.Ordinal))
		{
			return !(method.ReturnType != typeof(void));
		}
		return false;
	}
}
