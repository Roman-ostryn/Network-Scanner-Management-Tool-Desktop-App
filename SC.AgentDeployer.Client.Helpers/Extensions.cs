using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using ScreenConnect;

namespace SC.AgentDeployer.Client.Helpers;

public static class Extensions
{
	public static string ConvertToUnsecureString(this SecureString securePassword)
	{
		if (securePassword == null)
		{
			throw new ArgumentNullException();
		}
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
			return Marshal.PtrToStringUni(intPtr);
		}
		finally
		{
			Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
		}
	}

	public static bool IsInRange(this IPAddress address, IPAddress startAddressInclusive, IPAddress endAddressInclusive)
	{
		if (address.AddressFamily == startAddressInclusive.AddressFamily && address.AddressFamily == endAddressInclusive.AddressFamily)
		{
			byte[] addressBytes = startAddressInclusive.GetAddressBytes();
			byte[] addressBytes2 = endAddressInclusive.GetAddressBytes();
			byte[] addressBytes3 = address.GetAddressBytes();
			for (int i = 0; i < addressBytes3.Length; i++)
			{
				if (addressBytes3[i] < addressBytes[i] || addressBytes3[i] > addressBytes2[i])
				{
					return false;
				}
			}
			return true;
		}
		throw new InvalidOperationException("Cannot compare IP Addresses with different AddressFamily properties");
	}

	public static void NotifyPropertyChanged<T>(this T obj, Expression<Func<T, object>> selector) where T : IRaisePropertyChanged
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (selector.NodeType != ExpressionType.Lambda)
		{
			throw new ArgumentException("Property selector must be a lambda.", "selector");
		}
		MemberExpression obj2 = GetMemberExpression(selector.Body) ?? throw new ArgumentException("Selector must be a member access expression", "selector");
		if (obj2.Member?.DeclaringType == null)
		{
			throw new InvalidOperationException("Property does not have a declaring type.");
		}
		string name = obj2.Member.Name;
		PropertyInfo property = obj2.Member.DeclaringType.GetProperty(name);
		if (property != null)
		{
			name = property.Name;
		}
		obj.NotifyPropertyChanged(name);
	}

	public static void SaveMsiStream(this MsiFile msiFile, Stream stream)
	{
		ReflectionExtensions.InvokeMethod(ReflectionExtensions.GetFieldValue((object)msiFile, "compoundFile"), "Save", new object[1] { stream });
	}

	private static MemberExpression GetMemberExpression(Expression expression)
	{
		if (expression.NodeType == ExpressionType.MemberAccess)
		{
			return (MemberExpression)expression;
		}
		if (expression.NodeType == ExpressionType.Convert)
		{
			return GetMemberExpression(((UnaryExpression)expression).Operand);
		}
		return null;
	}
}
