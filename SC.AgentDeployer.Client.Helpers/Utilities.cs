using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using SC.AgentDeployer.Client.Models;

namespace SC.AgentDeployer.Client.Helpers;

public class Utilities
{
	private const int maxTimesToCheckClientStatusUponDeployment = 10;

	public const string InstallerName = "SC.AgentDeployer.Installer";

	public static IEnumerable<IPAddress> GetSubnetIPAddresses()
	{
		return from _ in (from _ in (from _ in NetworkInterface.GetAllNetworkInterfaces()
					where _.OperationalStatus == OperationalStatus.Up
					select _).SelectMany((NetworkInterface _) => _.GetIPProperties().UnicastAddresses)
				where _.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(_.Address)
				select new
				{
					AddressInt = (uint)IPAddress.HostToNetworkOrder(BitConverter.ToInt32(_.Address.GetAddressBytes(), 0)),
					SubnetMaskInt = (uint)IPAddress.HostToNetworkOrder(BitConverter.ToInt32(_.IPv4Mask.GetAddressBytes(), 0)),
					SubnetBitCount = (from __ in new BitArray(_.IPv4Mask.GetAddressBytes()).Cast<bool>().Select((bool b, int i) => new { b, i })
						where !__.b
						select __.i).First()
				}).SelectMany(_ => from __ in Enumerable.Range(0, (int)Math.Pow(2.0, 32 - _.SubnetBitCount))
				select (_.AddressInt & _.SubnetMaskInt) | (uint)__)
			select (uint)IPAddress.HostToNetworkOrder((int)_) into _
			select new IPAddress(_);
	}

	public static Dictionary<IPAddress, string> GetEndpointInfosFromSubnet()
	{
		return GetSubnetIPAddresses().Distinct().ToDictionary((IPAddress _) => _, (IPAddress _) => (string)null);
	}

	public static BinaryReader OpenReaderToExeFilePayloadStart(Type typeInExe)
	{
		try
		{
			FileStream fileStream = File.OpenRead(typeInExe.Assembly.Location);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			fileStream.Seek(-4L, SeekOrigin.End);
			int num = binaryReader.ReadInt32();
			fileStream.Seek(-num, SeekOrigin.End);
			return binaryReader;
		}
		catch
		{
			throw;
		}
	}

	public static Dictionary<IPAddress, string> GetEndpointInfosFromArpTable()
	{
		return IPHelper.GetAllDevicesOnLAN().ToDictionary((KeyValuePair<IPAddress, PhysicalAddress> _) => _.Key, (KeyValuePair<IPAddress, PhysicalAddress> _) => (string)null);
	}

	public static ClientServiceStatus GetClientServiceStatusUsingCredentials(IPAddress address, string dname, string uname, SecureString pw, string publicKeyFingerprint)
	{
		IntPtr phToken = default(IntPtr);
		try
		{
			if (!WindowsNative.LogonUser(uname, dname, pw.ConvertToUnsecureString(), 2, 0, out phToken))
			{
				return ClientServiceStatus.InvalidCredentials;
			}
			using WindowsIdentity windowsIdentity = new WindowsIdentity(phToken);
			using (windowsIdentity.Impersonate())
			{
				return GetClientServiceStatus(address, publicKeyFingerprint);
			}
		}
		catch (Exception)
		{
			return ClientServiceStatus.InvalidCredentials;
		}
		finally
		{
			WindowsNative.CloseHandle(phToken);
		}
	}

	public static ClientServiceStatus GetClientServiceStatus(IPAddress address, string publicKeyFingerprint)
	{
		try
		{
			string lpServiceName = $"ScreenConnect Client ({publicKeyFingerprint})";
			IntPtr intPtr = WindowsNative.OpenSCManager(address.ToString(), null, SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
			if (intPtr == IntPtr.Zero)
			{
				switch (Marshal.GetLastWin32Error())
				{
				case 5:
					return ClientServiceStatus.AccessDenied;
				case 1065:
					return ClientServiceStatus.ScmDoesNotExist;
				default:
					if (EndpointRespondedToArpRequest(address))
					{
						return ClientServiceStatus.PossibleNonWindowsMachine;
					}
					return ClientServiceStatus.NoResponseToArp;
				}
			}
			IntPtr intPtr2 = WindowsNative.OpenService(intPtr, lpServiceName, SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
			if (intPtr2 == IntPtr.Zero)
			{
				return Marshal.GetLastWin32Error() switch
				{
					5 => ClientServiceStatus.AccessDenied, 
					6 => ClientServiceStatus.InvalidClientServiceHandle, 
					123 => ClientServiceStatus.InvalidClientServiceName, 
					1060 => ClientServiceStatus.ClientServiceDoesNotExist, 
					_ => ClientServiceStatus.ClientServiceOtherError, 
				};
			}
			try
			{
				return GetServiceStatus(intPtr2) switch
				{
					5 => ClientServiceStatus.ServiceContinuePending, 
					6 => ClientServiceStatus.ServicePausePending, 
					7 => ClientServiceStatus.ServicePaused, 
					4 => ClientServiceStatus.ServiceRunning, 
					2 => ClientServiceStatus.ServiceStartPending, 
					3 => ClientServiceStatus.ServiceStopPending, 
					1 => ClientServiceStatus.ServiceStopped, 
					_ => ClientServiceStatus.UnknownStatus, 
				};
			}
			finally
			{
				WindowsNative.CloseServiceHandle(intPtr2);
				WindowsNative.CloseServiceHandle(intPtr);
			}
		}
		catch (Exception)
		{
			return ClientServiceStatus.ClientNotInstalledError;
		}
	}

	public static DeployClientResult DeployClientUsingCredentials(IPAddress address, string dname, string uname, SecureString pw, IDictionary<string, byte[]> fileMap, string publicKeyFingerprint)
	{
		IntPtr phToken = default(IntPtr);
		try
		{
			if (!WindowsNative.LogonUser(uname, dname, pw.ConvertToUnsecureString(), 2, 0, out phToken))
			{
				return DeployClientResult.InvalidCredentials;
			}
			using WindowsIdentity windowsIdentity = new WindowsIdentity(phToken);
			using (windowsIdentity.Impersonate())
			{
				return DeployClient(address, fileMap, publicKeyFingerprint);
			}
		}
		catch (Exception)
		{
			return DeployClientResult.InvalidCredentials;
		}
		finally
		{
			WindowsNative.CloseHandle(phToken);
		}
	}

	public static DeployClientResult DeployClient(IPAddress address, IDictionary<string, byte[]> fileMap, string publicKeyFingerprint)
	{
		try
		{
			IntPtr intPtr = WindowsNative.OpenSCManager(address.ToString(), null, SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
			if (intPtr == IntPtr.Zero)
			{
				switch (Marshal.GetLastWin32Error())
				{
				case 5:
					return DeployClientResult.AccessDenied;
				case 1065:
					return DeployClientResult.ScmDoesNotExist;
				default:
					if (EndpointRespondedToArpRequest(address))
					{
						return DeployClientResult.PossibleNonWindowsMachine;
					}
					return DeployClientResult.NoResponseToArp;
				}
			}
			using (FileStream fileStream = File.OpenWrite(string.Format("\\\\{0}\\admin$\\temp\\{1}.msi", address, "SC.AgentDeployer.Installer")))
			{
				byte[] array = fileMap[string.Format("{0}.msi", "SC.AgentDeployer.Installer")];
				fileStream.Write(array, 0, array.Length);
			}
			using (FileStream fileStream2 = File.OpenWrite(string.Format("\\\\{0}\\admin$\\temp\\{1}.exe", address, "SC.AgentDeployer.Installer")))
			{
				byte[] array2 = fileMap[string.Format("{0}.exe", "SC.AgentDeployer.Installer")];
				fileStream2.Write(array2, 0, array2.Length);
			}
			using (FileStream fileStream3 = File.OpenWrite(string.Format("\\\\{0}\\admin$\\temp\\{1}.exe.config", address, "SC.AgentDeployer.Installer")))
			{
				byte[] array3 = fileMap[string.Format("{0}.exe.config", "SC.AgentDeployer.Installer")];
				fileStream3.Write(array3, 0, array3.Length);
			}
			IntPtr intPtr2 = WindowsNative.OpenService(intPtr, "SC.AgentDeployer.Installer", SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
			if (intPtr2 == IntPtr.Zero)
			{
				string lpPathName = Path.Combine(WindowsNative.NetShareGetPath($"\\\\{address}", "admin$"), string.Format("temp\\{0}.exe", "SC.AgentDeployer.Installer"));
				intPtr2 = WindowsNative.CreateService(intPtr, "SC.AgentDeployer.Installer", "SC Agent Deployer Service", SERVICE_ACCESS.SERVICE_ALL_ACCESS, SERVICE_TYPES.SERVICE_WIN32_OWN_PROCESS, SERVICE_START_TYPES.SERVICE_DEMAND_START, SERVICE_ERROR_CONTROL.SERVICE_ERROR_NORMAL, lpPathName, null, IntPtr.Zero, null, null, null);
				if (intPtr2 == IntPtr.Zero)
				{
					return Marshal.GetLastWin32Error() switch
					{
						5 => DeployClientResult.AccessDenied, 
						123 => DeployClientResult.InvalidNetworkerServiceName, 
						1059 => DeployClientResult.NetworkerServiceCircularDependency, 
						1072 => DeployClientResult.NetworkerServiceMarkedForDeletion, 
						1073 => DeployClientResult.NetworkerServiceAlreadyExists, 
						1078 => DeployClientResult.DuplicateServiceName, 
						_ => DeployClientResult.NetworkerServiceOtherError, 
					};
				}
			}
			WindowsNative.StartService(intPtr2, 0, null);
			WindowsNative.DeleteService(intPtr2);
			WindowsNative.CloseServiceHandle(intPtr2);
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			do
			{
				zero = WindowsNative.OpenService(intPtr, $"ScreenConnect Client ({publicKeyFingerprint})", SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
				num++;
				if (zero == IntPtr.Zero)
				{
					Thread.Sleep(1000);
				}
			}
			while (zero == IntPtr.Zero && num < 10);
			if (zero == IntPtr.Zero)
			{
				return DeployClientResult.UnknownStatus;
			}
			try
			{
				return GetServiceStatus(zero) switch
				{
					5 => DeployClientResult.ServiceContinuePending, 
					6 => DeployClientResult.ServicePausePending, 
					7 => DeployClientResult.ServicePaused, 
					4 => DeployClientResult.ServiceRunning, 
					2 => DeployClientResult.ServiceStartPending, 
					3 => DeployClientResult.ServiceStopPending, 
					1 => DeployClientResult.ServiceStopped, 
					_ => DeployClientResult.UnknownStatus, 
				};
			}
			finally
			{
				WindowsNative.CloseServiceHandle(zero);
				WindowsNative.CloseServiceHandle(intPtr);
			}
		}
		catch (Exception)
		{
			return DeployClientResult.ClientInstallFailure;
		}
	}

	public static ClientServiceStatus StartClientServiceUsingCredentials(IPAddress address, string dname, string uname, SecureString pw, string publicKeyFingerprint)
	{
		IntPtr phToken = default(IntPtr);
		try
		{
			if (!WindowsNative.LogonUser(uname, dname, pw.ConvertToUnsecureString(), 2, 0, out phToken))
			{
				return ClientServiceStatus.InvalidCredentials;
			}
			using WindowsIdentity windowsIdentity = new WindowsIdentity(phToken);
			using (windowsIdentity.Impersonate())
			{
				return StartClientService(address, publicKeyFingerprint);
			}
		}
		catch (Exception)
		{
			return ClientServiceStatus.InvalidCredentials;
		}
		finally
		{
			WindowsNative.CloseHandle(phToken);
		}
	}

	public static ClientServiceStatus StartClientService(IPAddress address, string publicKeyFingerprint)
	{
		try
		{
			string lpServiceName = $"ScreenConnect Client ({publicKeyFingerprint})";
			IntPtr intPtr = WindowsNative.OpenSCManager(address.ToString(), null, SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
			if (intPtr == IntPtr.Zero)
			{
				switch (Marshal.GetLastWin32Error())
				{
				case 5:
					return ClientServiceStatus.AccessDenied;
				case 1065:
					return ClientServiceStatus.ScmDoesNotExist;
				default:
					if (EndpointRespondedToArpRequest(address))
					{
						return ClientServiceStatus.PossibleNonWindowsMachine;
					}
					return ClientServiceStatus.NoResponseToArp;
				}
			}
			IntPtr intPtr2 = WindowsNative.OpenService(intPtr, lpServiceName, SCM_ACCESS.SC_MANAGER_ALL_ACCESS);
			if (intPtr2 == IntPtr.Zero)
			{
				return Marshal.GetLastWin32Error() switch
				{
					5 => ClientServiceStatus.AccessDenied, 
					6 => ClientServiceStatus.InvalidClientServiceHandle, 
					123 => ClientServiceStatus.InvalidClientServiceName, 
					1060 => ClientServiceStatus.ClientServiceDoesNotExist, 
					_ => ClientServiceStatus.ClientServiceOtherError, 
				};
			}
			try
			{
				switch (GetServiceStatus(intPtr2))
				{
				case 5:
					return ClientServiceStatus.ServiceContinuePending;
				case 6:
					return ClientServiceStatus.ServicePausePending;
				case 4:
					return ClientServiceStatus.ServiceRunning;
				case 2:
					return ClientServiceStatus.ServiceStartPending;
				case 3:
					return ClientServiceStatus.ServiceStopPending;
				case 1:
				case 7:
					if (WindowsNative.StartService(intPtr2, 0, null))
					{
						return ClientServiceStatus.ServiceStarted;
					}
					return ClientServiceStatus.ServiceNotStarted;
				default:
					return ClientServiceStatus.UnknownStatus;
				}
			}
			catch (Exception)
			{
				return ClientServiceStatus.UnknownStatus;
			}
			finally
			{
				WindowsNative.CloseServiceHandle(intPtr2);
				WindowsNative.CloseServiceHandle(intPtr);
			}
		}
		catch (Exception)
		{
			return ClientServiceStatus.ClientNotInstalledError;
		}
	}

	private static bool EndpointRespondedToArpRequest(IPAddress address)
	{
		uint destIP = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
		byte[] array = new byte[6];
		int PhyAddrLen = array.Length;
		if (WindowsNative.SendARP(destIP, 0u, array, ref PhyAddrLen) != 0)
		{
			return false;
		}
		return true;
	}

	private static int GetServiceStatus(IntPtr serviceHandle)
	{
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			int bytesNeeded = 0;
			WindowsNative.QueryServiceStatusEx(serviceHandle, 0, intPtr, bytesNeeded, out bytesNeeded);
			intPtr = Marshal.AllocHGlobal(bytesNeeded);
			WindowsNative.QueryServiceStatusEx(serviceHandle, 0, intPtr, bytesNeeded, out bytesNeeded);
			return ((WindowsNative.SERVICE_STATUS_PROCESS)Marshal.PtrToStructure(intPtr, typeof(WindowsNative.SERVICE_STATUS_PROCESS))).currentState;
		}
		finally
		{
			if (!intPtr.Equals(IntPtr.Zero))
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}
}
