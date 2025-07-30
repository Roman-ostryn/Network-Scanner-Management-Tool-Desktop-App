using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using SC.AgentDeployer.Client.Helpers;
using ScreenConnect;

namespace SC.AgentDeployer.Client.Services;

public class ClientPayloadService : SingletonServiceBase<ClientPayloadService>
{
	private const string ResourceKeyPrefix = "LabelResources";

	public IDictionary<string, string> ClientStringMap { get; private set; }

	public IDictionary<string, byte[]> ClientFileMap { get; private set; }

	private static string InstallerFileName => "SC.AgentDeployer.Installer.msi";

	public void Initialize()
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		BinaryReader payloadReader = Utilities.OpenReaderToExeFilePayloadStart(typeof(Utilities));
		try
		{
			ClientStringMap = Extensions.ToDictionary<string, string>(from it in Enumerable.Range(0, payloadReader.ReadInt32())
				select Extensions.CreateKeyValuePair<string, string>(payloadReader.ReadString(), payloadReader.ReadString()), false);
			ClientFileMap = Extensions.ToDictionary<string, byte[]>(from it in Enumerable.Range(0, payloadReader.ReadInt32())
				select Extensions.CreateKeyValuePair<string, byte[]>(payloadReader.ReadString(), payloadReader.ReadBytes(payloadReader.ReadInt32())), false);
		}
		finally
		{
			if (payloadReader != null)
			{
				((IDisposable)payloadReader).Dispose();
			}
		}
		Extensions.ForEach2<KeyValuePair<string, string>>(ClientStringMap.Where((KeyValuePair<string, string> it) => it.Key.StartsWith("LabelResources")), (Proc<KeyValuePair<string, string>>)delegate(KeyValuePair<string, string> it)
		{
			Application.Current.Resources[it.Key] = it.Value;
		});
		if (ClientFileMap == null || !ClientFileMap.ContainsKey(InstallerFileName))
		{
			return;
		}
		using MemoryStream memoryStream = new MemoryStream(ClientFileMap[InstallerFileName]);
		MsiFile val = new MsiFile((Stream)memoryStream, string.Empty);
		string text = Extensions.As<string>(val.GetRowColumnData("Property", (object)"SERVICE_CLIENT_LAUNCH_PARAMETERS", 1).FirstOrDefault());
		if (string.IsNullOrEmpty(text))
		{
			text = Extensions.As<string>(val.GetRowColumnData("Property", (object)"SERVICE_ARGUMENTS", 1).FirstOrDefault());
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new InvalidOperationException("Query string could not be found in msi file");
		}
		SingletonServiceBase<ClientInfoService>.Instance.ClientLaunchParameters = ClientLaunchParameters.FromQueryString(text);
	}

	public void UpdateClientLaunchParameters()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (ClientFileMap == null || !ClientFileMap.ContainsKey(InstallerFileName) || SingletonServiceBase<ClientInfoService>.Instance.ClientLaunchParameters == null)
		{
			return;
		}
		using MemoryStream memoryStream = new MemoryStream(ClientFileMap[InstallerFileName]);
		using MemoryStream memoryStream2 = new MemoryStream();
		MsiFile val = new MsiFile((Stream)memoryStream, string.Empty);
		string text = ClientLaunchParameters.ToQueryString(SingletonServiceBase<ClientInfoService>.Instance.ClientLaunchParameters);
		val.UpdateRows("Property", (object)"SERVICE_CLIENT_LAUNCH_PARAMETERS", 1, (object)text);
		val.UpdateRows("Property", (object)"SERVICE_ARGUMENTS", 1, (object)text);
		Extensions.SaveMsiStream(val, memoryStream2);
		ClientFileMap[InstallerFileName] = memoryStream2.ToArray();
	}
}
