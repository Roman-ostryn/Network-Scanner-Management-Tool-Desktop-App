using System;
using System.Runtime.InteropServices;
using SC.AgentDeployer.Client.Models;

namespace SC.AgentDeployer.Client.Helpers;

public class WindowsNative
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct SERVICE_STATUS_PROCESS
	{
		public int serviceType;

		public int currentState;

		public int controlsAccepted;

		public int win32ExitCode;

		public int serviceSpecificExitCode;

		public int checkPoint;

		public int waitHint;

		public int processID;

		public int serviceFlags;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct SHARE_INFO_2
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string shi2_netname;

		public uint shi2_type;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string shi2_remark;

		public uint shi2_permissions;

		public uint shi2_max_uses;

		public uint shi2_current_uses;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string shi2_path;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string shi2_passwd;
	}

	private const int ERROR_ACCESS_DENIED = 5;

	private const int ERROR_INVALID_LEVEL = 124;

	private const int ERROR_INVALID_PARAMETER = 87;

	private const int ERROR_MORE_DATA = 234;

	private const int ERROR_NOT_ENOUGH_MEMORY = 8;

	private const int NERR_BufTooSmall = 2123;

	private const int NERR_NetNameNotFound = 2310;

	private const int NERR_Success = 0;

	[DllImport("coredll.dll", SetLastError = true)]
	private static extern int GetLastError();

	[DllImport("iphlpapi.dll", ExactSpelling = true)]
	public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

	[DllImport("kernel32", SetLastError = true)]
	public static extern bool CloseHandle(IntPtr hObject);

	[DllImport("Netapi32", CharSet = CharSet.Auto)]
	private static extern int NetApiBufferFree(IntPtr Buffer);

	[DllImport("Netapi32", CharSet = CharSet.Auto)]
	private static extern int NetShareGetInfo([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.LPWStr)] string netname, int level, ref IntPtr bufptr);

	[DllImport("Netapi32.dll", SetLastError = true)]
	public static extern int NetEnumerateComputerNames([MarshalAs(UnmanagedType.LPWStr)] string server_name, NET_COMPUTER_NAME_TYPE name_type, int reserved, out int entry_count, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] out string[] computer_names);

	[DllImport("advapi32", SetLastError = true)]
	public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

	[DllImport("advapi32", SetLastError = true)]
	public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, SCM_ACCESS scParameter);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CloseServiceHandle(IntPtr hSCObject);

	[DllImport("advapi32", SetLastError = true)]
	public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName, SERVICE_ACCESS dwDesiredAccess, SERVICE_TYPES dwServiceType, SERVICE_START_TYPES dwStartType, SERVICE_ERROR_CONTROL dwErrorControl, string lpPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, SCM_ACCESS dwDesiredAccess);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool QueryServiceStatusEx(IntPtr serviceHandle, int infoLevel, IntPtr buffer, int bufferSize, out int bytesNeeded);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DeleteService(IntPtr hService);

	[DllImport("advapi32", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool StartService(IntPtr hService, int dwNumServiceArgs, string[] lpServiceArgVectors);

	public static string NetShareGetPath(string serverName, string netName)
	{
		string text = null;
		IntPtr bufptr = IntPtr.Zero;
		int num = NetShareGetInfo(serverName, netName, 2, ref bufptr);
		if (num == 0)
		{
			text = ((SHARE_INFO_2)Marshal.PtrToStructure(bufptr, typeof(SHARE_INFO_2))).shi2_path;
			NetApiBufferFree(bufptr);
			return text;
		}
		return FormatMessage(num);
	}

	private static string FormatMessage(int errCode)
	{
		return errCode switch
		{
			5 => "The user does not have access to the requested information.", 
			124 => "The value specified for the level parameter is invalid.", 
			87 => "The specified parameter is invalid.", 
			234 => "More entries are available. Specify a large enough buffer to receive all entries.", 
			8 => "Insufficient memory is available.", 
			2123 => "The supplied buffer is too small.", 
			2310 => "The share name does not exist.", 
			_ => null, 
		};
	}

	public static string TryGetComputerName(string host)
	{
		NetEnumerateComputerNames(host, NET_COMPUTER_NAME_TYPE.NetAllComputerNames, 0, out var _, out var computer_names);
		if (computer_names != null && computer_names.Length != 0)
		{
			return computer_names[0];
		}
		return "Unknown";
	}
}
