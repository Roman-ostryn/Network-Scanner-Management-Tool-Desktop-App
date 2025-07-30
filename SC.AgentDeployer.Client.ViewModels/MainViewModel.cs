using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Win32;
using SC.AgentDeployer.Client.Commands;
using SC.AgentDeployer.Client.Helpers;
using SC.AgentDeployer.Client.Models;
using SC.AgentDeployer.Client.Properties;
using SC.AgentDeployer.Client.Services;
using SC.AgentDeployer.Client.Views;

namespace SC.AgentDeployer.Client.ViewModels;

public class MainViewModel : ViewModelBase
{
	private const string defaultSubnetFilter = "*.*.*.*";

	private bool isLoading;

	private string statusMessage;

	private string currentUser;

	private string localUser;

	private ICollection<Machine> machineCollection;

	private ObservableCollection<Machine> filteredMachines;

	private string selectedNetworkScanType;

	private bool allIpAddressesSelected;

	private string subnetFilter;

	private CancellationTokenSource cancellationTokenSource;

	private readonly ResourceManager resourceManager = Resources.ResourceManager;

	private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

	protected CancellationTokenSource CancellationTokenSource
	{
		get
		{
			return cancellationTokenSource;
		}
		set
		{
			cancellationTokenSource = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.CancellationTokenSource);
		}
	}

	public bool IsLoading
	{
		get
		{
			return isLoading;
		}
		set
		{
			isLoading = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.IsLoading);
		}
	}

	public string StatusMessage
	{
		get
		{
			return statusMessage;
		}
		set
		{
			statusMessage = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.StatusMessage);
		}
	}

	public string CurrentUser
	{
		get
		{
			return currentUser;
		}
		set
		{
			currentUser = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.CurrentUser);
		}
	}

	public string LocalUser
	{
		get
		{
			return localUser;
		}
		set
		{
			localUser = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.LocalUser);
		}
	}

	public ObservableCollection<Machine> FilteredMachines
	{
		get
		{
			return filteredMachines;
		}
		set
		{
			filteredMachines = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.FilteredMachines);
		}
	}

	public string SelectedNetworkScanType
	{
		get
		{
			return selectedNetworkScanType;
		}
		set
		{
			selectedNetworkScanType = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.SelectedNetworkScanType);
		}
	}

	public bool AllIpAddressesSelected
	{
		get
		{
			return allIpAddressesSelected;
		}
		set
		{
			allIpAddressesSelected = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.AllIpAddressesSelected);
		}
	}

	public string SubnetFilter
	{
		get
		{
			return subnetFilter;
		}
		set
		{
			subnetFilter = value;
			this.NotifyPropertyChanged((MainViewModel _) => _.SubnetFilter);
			this.NotifyPropertyChanged((MainViewModel _) => _.FilterIpRangeCommand);
		}
	}

	public ICommand ViewDomainCredentialsCommand { get; private set; }

	public ICommand EditClientDetailsCommand { get; private set; }

	public ICommand ToggleIpRowSelectionCommand { get; private set; }

	public ICommand SelectAllRowsInIpTableCommand { get; private set; }

	public ICommand UnselectAllRowsInIpTableCommand { get; private set; }

	public ICommand FilterIpRangeCommand { get; private set; }

	public ICommand ResetSubnetFilterCommand { get; private set; }

	public ICommand EnsureGlobalAddressCheckboxSyncedCommand { get; private set; }

	public ICommand CheckStatusAsyncCommand { get; private set; }

	public ICommand StartClientsAsyncCommand { get; private set; }

	public ICommand DeployClientsAsyncCommand { get; private set; }

	public ICommand CancelCommand { get; private set; }

	public ICommand SaveReportCommand { get; private set; }

	public ICommand LoadFromSubnetCommand { get; private set; }

	public ICommand LoadFromArpTableCommand { get; private set; }

	public ICommand ShowContextMenuCommand { get; private set; }

	public ICommand SetUserToCurrentCommand { get; private set; }

	public event EventHandler OnClose;

	protected override void Initialize()
	{
		IsLoading = true;
		try
		{
			machineCollection = new List<Machine>();
			filteredMachines = new ObservableCollection<Machine>();
			selectedNetworkScanType = "None";
			subnetFilter = "*.*.*.*";
			localUser = ((!string.IsNullOrEmpty(Environment.UserDomainName)) ? (Environment.UserDomainName + "\\" + Environment.UserName) : Environment.UserName);
			EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnKeyboardFocus));
			UpdateCurrentUser();
			StatusMessage = GetStringResource("StatusProcessingPayload");
			SingletonServiceBase<DomainCredentialsService>.Instance.PropertyChanged += delegate
			{
				UpdateCurrentUser();
				StatusMessage = GetStringResource("StatusDomainCredentialsSaved");
			};
			SingletonServiceBase<ClientInfoService>.Instance.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
			{
				if (args?.PropertyName == "ClientLaunchParameters")
				{
					TryUpdateClientLaunchParameters();
					StatusMessage = GetStringResource("ClientDetailsUpdated");
				}
			};
			SingletonServiceBase<ClientPayloadService>.Instance.Initialize();
			StatusMessage = GetStringResource("StatusFinishedProcessingPayload");
		}
		catch (Exception ex)
		{
			StatusMessage = ex.Message;
		}
		IsLoading = false;
	}

	protected override void InitializeCommands()
	{
		ViewDomainCredentialsCommand = new RelayCommand(delegate
		{
			ViewDomainCredentialsWindow();
		}, (object _) => !IsLoading);
		EditClientDetailsCommand = new RelayCommand(delegate
		{
			EditClientDetails();
		}, (object _) => ClientDetailsLoaded() && !IsLoading);
		ToggleIpRowSelectionCommand = new RelayCommand(delegate
		{
			ToggleIpRowSelection();
		}, (object _) => !IsLoading);
		SelectAllRowsInIpTableCommand = new RelayCommand(delegate
		{
			SelectAllRowsInIpTable();
		}, (object _) => !IsLoading);
		UnselectAllRowsInIpTableCommand = new RelayCommand(delegate
		{
			UnselectAllRowsInIpTable();
		}, (object _) => !IsLoading);
		FilterIpRangeCommand = new RelayCommand(delegate
		{
			FilterIpRange();
		}, (object _) => !IsLoading && CanFilterIpRange());
		ResetSubnetFilterCommand = new RelayCommand(delegate
		{
			ResetSubnetFilter();
		}, (object _) => !IsLoading);
		EnsureGlobalAddressCheckboxSyncedCommand = new RelayCommand(delegate
		{
			EnsureGlobalAddressCheckboxSynced();
		}, (object _) => !IsLoading);
		CheckStatusAsyncCommand = new RelayCommand(delegate
		{
			CheckStatusAsync();
		}, (object _) => ClientDetailsLoaded() && HasSelectedMachines() && !IsLoading);
		StartClientsAsyncCommand = new RelayCommand(delegate
		{
			StartClientsAsync();
		}, (object _) => ClientDetailsLoaded() && HasSelectedMachines() && !IsLoading);
		DeployClientsAsyncCommand = new RelayCommand(delegate
		{
			DeployClientsAsync();
		}, (object _) => ClientDetailsLoaded() && HasSelectedMachines() && !IsLoading);
		CancelCommand = new RelayCommand(delegate
		{
			Cancel();
		}, (object _) => CanCancel());
		SaveReportCommand = new RelayCommand(delegate
		{
			SaveReport();
		}, (object _) => CanSaveReport());
		ShowContextMenuCommand = new RelayCommand<Button>(delegate(Button _)
		{
			ShowContextMenu(_);
		}, (Button _) => !IsLoading);
		LoadFromSubnetCommand = new RelayCommand(delegate
		{
			LoadFromSubnet();
		}, (object _) => !IsLoading);
		LoadFromArpTableCommand = new RelayCommand(delegate
		{
			LoadFromArpTable();
		}, (object _) => !IsLoading);
		SetUserToCurrentCommand = new RelayCommand(delegate
		{
			SetUserToCurrent();
		}, (object _) => !IsLoading);
	}

	public override void OnClosing()
	{
		this.OnClose?.Invoke(this, EventArgs.Empty);
	}

	private void TryUpdateClientLaunchParameters()
	{
		try
		{
			IsLoading = true;
			SingletonServiceBase<ClientPayloadService>.Instance.UpdateClientLaunchParameters();
		}
		catch (Exception ex)
		{
			StatusMessage = ex.Message;
		}
		finally
		{
			IsLoading = false;
		}
	}

	private void ViewDomainCredentialsWindow()
	{
		DomainCredentialsWindow domainCredentialsWindow = new DomainCredentialsWindow();
		OnClose += delegate
		{
			domainCredentialsWindow.Close();
		};
		domainCredentialsWindow.ShowDialog();
	}

	private void EditClientDetails()
	{
		if (ClientDetailsLoaded())
		{
			SingletonServiceBase<ClientInfoService>.Instance.ClientServiceName = string.Format("ScreenConnect Client ({0})", SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
			SingletonServiceBase<ClientInfoService>.Instance.ClientServiceVersion = SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["ClientVersion"];
			SingletonServiceBase<ClientInfoService>.Instance.ClientServiceRelayUrl = SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["RelayUri"];
			ClientInfoWindow clientInfoWindow = new ClientInfoWindow();
			OnClose += delegate
			{
				clientInfoWindow.Close();
			};
			clientInfoWindow.ShowDialog();
		}
		else
		{
			StatusMessage = "Payload does not contain client information";
		}
	}

	private void SetUserToCurrent()
	{
		SingletonServiceBase<DomainCredentialsService>.Instance.DomainName = null;
		SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser = null;
		SingletonServiceBase<DomainCredentialsService>.Instance.DomainPassword = null;
		UpdateCurrentUser();
	}

	private bool ClientDetailsLoaded()
	{
		if (SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap != null && SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap.Count > 0)
		{
			return true;
		}
		return false;
	}

	private void ShowContextMenu(Button button)
	{
		if (button != null && button.ContextMenu != null)
		{
			button.ContextMenu.IsOpen = true;
			button.ContextMenu.IsEnabled = true;
			button.ContextMenu.PlacementTarget = button;
			button.ContextMenu.Placement = PlacementMode.Bottom;
		}
	}

	private void LoadFromSubnet()
	{
		try
		{
			IsLoading = true;
			AllIpAddressesSelected = false;
			machineCollection.Clear();
			foreach (KeyValuePair<IPAddress, string> item in Utilities.GetEndpointInfosFromSubnet())
			{
				machineCollection.Add(new Machine
				{
					Ip = item.Key.ToString(),
					Status = item.Value,
					State = MachineQueryState.None
				});
			}
			SelectedNetworkScanType = "Subnet";
			FilterIpRange(suppressStatusMessage: true);
			StatusMessage = GetStringResource("StatusLoadedFromSubnet");
		}
		catch (Exception ex)
		{
			SelectedNetworkScanType = "None";
			StatusMessage = ex.Message;
		}
		finally
		{
			IsLoading = false;
		}
	}

	private void LoadFromArpTable()
	{
		try
		{
			IsLoading = true;
			AllIpAddressesSelected = false;
			machineCollection.Clear();
			foreach (KeyValuePair<IPAddress, string> item in Utilities.GetEndpointInfosFromArpTable())
			{
				machineCollection.Add(new Machine
				{
					Ip = item.Key.ToString(),
					Status = item.Value,
					State = MachineQueryState.None
				});
			}
			SelectedNetworkScanType = "ARP Table";
			FilterIpRange(suppressStatusMessage: true);
			StatusMessage = GetStringResource("StatusLoadedFromArpTable");
		}
		catch (Exception ex)
		{
			SelectedNetworkScanType = "None";
			StatusMessage = ex.Message;
		}
		finally
		{
			IsLoading = false;
		}
	}

	private void ToggleIpRowSelection()
	{
		if (allIpAddressesSelected)
		{
			UnselectAllRowsInIpTable();
		}
		else
		{
			SelectAllRowsInIpTable();
		}
	}

	private void SelectAllRowsInIpTable()
	{
		IsLoading = true;
		try
		{
			AllIpAddressesSelected = true;
			foreach (Machine filteredMachine in FilteredMachines)
			{
				filteredMachine.IsSelected = true;
			}
		}
		finally
		{
			StatusMessage = GetStringResource("StatusSelectAllComplete");
			IsLoading = false;
		}
	}

	private void UnselectAllRowsInIpTable()
	{
		IsLoading = true;
		try
		{
			AllIpAddressesSelected = false;
			foreach (Machine filteredMachine in FilteredMachines)
			{
				filteredMachine.IsSelected = false;
			}
		}
		finally
		{
			StatusMessage = GetStringResource("StatusClearAllComplete");
			IsLoading = false;
		}
	}

	private void EnsureGlobalAddressCheckboxSynced()
	{
		AllIpAddressesSelected = FilteredMachines.All((Machine _) => _.IsSelected);
	}

	private void CheckStatusAsync()
	{
		if (!ClientDetailsLoaded())
		{
			StatusMessage = GetStringResource("ClientDetailsNotLoaded");
		}
		else
		{
			if (!IsAtLeastOneIpSelected(GetStringResource("StatusSelectIpBeforeCheckStatus")))
			{
				return;
			}
			IsLoading = true;
			ClearSelectedMachineStatuses();
			CancellationTokenSource = new CancellationTokenSource();
			List<Task> statusTasks = new List<Task>();
			try
			{
				foreach (Machine machine in FilteredMachines.Where((Machine _) => _.IsSelected))
				{
					StatusMessage = string.Format(GetStringResource("StatusLoadingClientStatusForIp"), machine.Ip);
					if (SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser != null)
					{
						statusTasks.Add(Task.Factory.StartNew(delegate
						{
							ReportProgress(delegate
							{
								machine.State = MachineQueryState.InProgress;
								machine.Status = GetStringResource("StatusCheckingClientStatus");
							});
							ClientServiceStatus clientStatus = Utilities.GetClientServiceStatusUsingCredentials(IPAddress.Parse(machine.Ip), SingletonServiceBase<DomainCredentialsService>.Instance.DomainName, SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser, SingletonServiceBase<DomainCredentialsService>.Instance.DomainPassword, SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
							string message = GetStringResource("ClientStatus" + clientStatus);
							ReportProgress(delegate
							{
								machine.State = MachineQueryState.Complete;
								machine.Status = message;
								machine.IsMachineCapableAndClientNotInstalled = clientStatus == ClientServiceStatus.ClientServiceDoesNotExist;
							});
						}, CancellationTokenSource.Token));
						continue;
					}
					statusTasks.Add(Task.Factory.StartNew(delegate
					{
						ReportProgress(delegate
						{
							machine.State = MachineQueryState.InProgress;
							machine.Status = GetStringResource("StatusCheckingClientStatus");
						});
						ClientServiceStatus clientServiceStatus = Utilities.GetClientServiceStatus(IPAddress.Parse(machine.Ip), SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
						string message = GetStringResource("ClientStatus" + clientServiceStatus);
						ReportProgress(delegate
						{
							machine.State = MachineQueryState.Complete;
							machine.Status = message;
						});
					}, CancellationTokenSource.Token));
				}
			}
			catch (Exception ex)
			{
				StatusMessage = ex.Message;
			}
			finally
			{
				try
				{
					Task.Factory.StartNew(delegate
					{
						try
						{
							Task.WaitAll(statusTasks.ToArray(), CancellationTokenSource.Token);
							ReportProgress(delegate
							{
								StatusMessage = GetStringResource("StatusLoadClientStatusFinished");
							});
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex5 = ex4;
							ReportProgress(delegate
							{
								StatusMessage = ex5.Message;
							});
						}
					}).ContinueWith(delegate
					{
						CancellationTokenSource = null;
						IsLoading = false;
						MarshalCallToMainThread(CommandManager.InvalidateRequerySuggested);
					});
				}
				catch (Exception ex2)
				{
					StatusMessage = ex2.Message;
					CancellationTokenSource = null;
					IsLoading = false;
				}
			}
		}
	}

	private void DeployClientsAsync()
	{
		if (!ClientDetailsLoaded())
		{
			StatusMessage = GetStringResource("ClientDetailsNotLoaded");
		}
		else
		{
			if (!IsAtLeastOneIpSelected(GetStringResource("StatusSelectIpBeforeDeployClient")))
			{
				return;
			}
			IsLoading = true;
			ClearSelectedMachineStatuses();
			CancellationTokenSource = new CancellationTokenSource();
			List<Task> deployTasks = new List<Task>();
			try
			{
				foreach (Machine machine in FilteredMachines.Where((Machine _) => _.IsSelected))
				{
					StatusMessage = string.Format(GetStringResource("StatusDeployingClientToIp"), machine.Ip);
					if (SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser != null)
					{
						deployTasks.Add(Task.Factory.StartNew(delegate
						{
							ReportProgress(delegate
							{
								machine.State = MachineQueryState.InProgress;
								machine.Status = GetStringResource("StatusAttemptingClientDeployment");
							});
							DeployClientResult deployClientResult = Utilities.DeployClientUsingCredentials(IPAddress.Parse(machine.Ip), SingletonServiceBase<DomainCredentialsService>.Instance.DomainName, SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser, SingletonServiceBase<DomainCredentialsService>.Instance.DomainPassword, SingletonServiceBase<ClientPayloadService>.Instance.ClientFileMap, SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
							string message = GetStringResource("DeployClientMessage" + deployClientResult);
							ReportProgress(delegate
							{
								machine.State = MachineQueryState.Complete;
								machine.Status = message;
							});
						}, CancellationTokenSource.Token));
						continue;
					}
					deployTasks.Add(Task.Factory.StartNew(delegate
					{
						ReportProgress(delegate
						{
							machine.State = MachineQueryState.InProgress;
							machine.Status = GetStringResource("StatusAttemptingClientDeployment");
						});
						DeployClientResult deployClientResult = Utilities.DeployClient(IPAddress.Parse(machine.Ip), SingletonServiceBase<ClientPayloadService>.Instance.ClientFileMap, SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
						string message = GetStringResource("DeployClientMessage" + deployClientResult);
						ReportProgress(delegate
						{
							machine.State = MachineQueryState.Complete;
							machine.Status = message;
						});
					}, CancellationTokenSource.Token));
				}
			}
			catch (Exception ex)
			{
				StatusMessage = ex.Message;
			}
			finally
			{
				try
				{
					StatusMessage = GetStringResource("StatusAttemptingClientDeployments");
					Task.Factory.StartNew(delegate
					{
						try
						{
							Task.WaitAll(deployTasks.ToArray(), CancellationTokenSource.Token);
							ReportProgress(delegate
							{
								StatusMessage = GetStringResource("StatusDeployClientFinished");
							});
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex5 = ex4;
							ReportProgress(delegate
							{
								StatusMessage = ex5.Message;
							});
						}
					}).ContinueWith(delegate
					{
						CancellationTokenSource = null;
						IsLoading = false;
						MarshalCallToMainThread(CommandManager.InvalidateRequerySuggested);
					});
				}
				catch (Exception ex2)
				{
					StatusMessage = ex2.Message;
					CancellationTokenSource = null;
					IsLoading = false;
				}
			}
		}
	}

	private void StartClientsAsync()
	{
		if (!ClientDetailsLoaded())
		{
			StatusMessage = GetStringResource("ClientDetailsNotLoaded");
		}
		else
		{
			if (!IsAtLeastOneIpSelected(GetStringResource("StatusSelectIpBeforeStartClient")))
			{
				return;
			}
			IsLoading = true;
			ClearSelectedMachineStatuses();
			CancellationTokenSource = new CancellationTokenSource();
			List<Task> startTasks = new List<Task>();
			try
			{
				foreach (Machine machine in FilteredMachines.Where((Machine _) => _.IsSelected))
				{
					StatusMessage = string.Format(GetStringResource("StatusStartingClientOnIp"), machine.Ip);
					if (SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser != null)
					{
						startTasks.Add(Task.Factory.StartNew(delegate
						{
							ReportProgress(delegate
							{
								machine.State = MachineQueryState.InProgress;
								machine.Status = GetStringResource("ClientStartStatusStartingService");
							});
							ClientServiceStatus clientServiceStatus = Utilities.StartClientServiceUsingCredentials(IPAddress.Parse(machine.Ip), SingletonServiceBase<DomainCredentialsService>.Instance.DomainName, SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser, SingletonServiceBase<DomainCredentialsService>.Instance.DomainPassword, SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
							string message = GetStringResource("ClientStartStatus" + clientServiceStatus);
							ReportProgress(delegate
							{
								machine.State = MachineQueryState.Complete;
								machine.Status = message;
							});
						}, CancellationTokenSource.Token));
						continue;
					}
					startTasks.Add(Task.Factory.StartNew(delegate
					{
						ReportProgress(delegate
						{
							machine.State = MachineQueryState.InProgress;
							machine.Status = GetStringResource("ClientStartStatusStartingService");
						});
						ClientServiceStatus clientServiceStatus = Utilities.StartClientService(IPAddress.Parse(machine.Ip), SingletonServiceBase<ClientPayloadService>.Instance.ClientStringMap["PublicKeyFingerprint"]);
						string message = GetStringResource("ClientStartStatus" + clientServiceStatus);
						ReportProgress(delegate
						{
							machine.State = MachineQueryState.Complete;
							machine.Status = message;
						});
					}, CancellationTokenSource.Token));
				}
			}
			catch (Exception ex)
			{
				StatusMessage = ex.Message;
			}
			finally
			{
				try
				{
					StatusMessage = GetStringResource("StatusStartingClients");
					Task.Factory.StartNew(delegate
					{
						try
						{
							Task.WaitAll(startTasks.ToArray(), CancellationTokenSource.Token);
							ReportProgress(delegate
							{
								StatusMessage = GetStringResource("StatusStartClientsFinished");
							});
						}
						catch (Exception ex3)
						{
							Exception ex4 = ex3;
							Exception ex5 = ex4;
							ReportProgress(delegate
							{
								StatusMessage = ex5.Message;
							});
						}
					}).ContinueWith(delegate
					{
						CancellationTokenSource = null;
						IsLoading = false;
						MarshalCallToMainThread(CommandManager.InvalidateRequerySuggested);
					});
				}
				catch (Exception ex2)
				{
					StatusMessage = ex2.Message;
					CancellationTokenSource = null;
					IsLoading = false;
				}
			}
		}
	}

	private void Cancel()
	{
		if (!CanCancel())
		{
			return;
		}
		try
		{
			CancellationTokenSource.Cancel();
			CancellationTokenSource.Dispose();
			foreach (Machine filteredMachine in FilteredMachines)
			{
				if (filteredMachine.State == MachineQueryState.InProgress)
				{
					filteredMachine.Status = "Cancelled";
					filteredMachine.State = MachineQueryState.Complete;
				}
			}
		}
		catch
		{
		}
		finally
		{
			CancellationTokenSource = null;
		}
	}

	private bool CanCancel()
	{
		if (CancellationTokenSource != null && !CancellationTokenSource.IsCancellationRequested)
		{
			return true;
		}
		return false;
	}

	private void SaveReport()
	{
		if (!CanSaveReport())
		{
			return;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IP Address, Status" + Environment.NewLine);
			foreach (Machine item in FilteredMachines.Where((Machine _) => !string.IsNullOrEmpty(_.Status)))
			{
				stringBuilder.Append(item.Ip + ", \"" + item.Status + "\" " + Environment.NewLine);
			}
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				FileName = "Agent Deployment Results",
				AddExtension = true,
				DefaultExt = "csv",
				Filter = "Comma Separated Values|*.csv"
			};
			bool? flag = saveFileDialog.ShowDialog();
			if (flag.HasValue && flag.Value)
			{
				File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
			}
		}
		catch (Exception ex)
		{
			StatusMessage = ex.Message;
		}
	}

	private bool CanSaveReport()
	{
		if (IsLoading)
		{
			return false;
		}
		if (FilteredMachines.Any((Machine _) => !string.IsNullOrEmpty(_.Status) && !string.IsNullOrEmpty(_.Ip)))
		{
			return true;
		}
		return false;
	}

	private bool HasSelectedMachines()
	{
		if (FilteredMachines != null && FilteredMachines.Any((Machine _) => _.IsSelected))
		{
			return true;
		}
		return false;
	}

	private Task ReportProgressAsync(Action action)
	{
		return Task.Factory.StartNew(action, CancellationTokenSource.Token, TaskCreationOptions.None, scheduler);
	}

	private void ReportProgress(Action action)
	{
		ReportProgressAsync(action).Wait();
	}

	private void UpdateCurrentUser()
	{
		if (!string.IsNullOrEmpty(SingletonServiceBase<DomainCredentialsService>.Instance.DomainName) && !string.IsNullOrEmpty(SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser))
		{
			CurrentUser = ((!string.IsNullOrEmpty(SingletonServiceBase<DomainCredentialsService>.Instance.DomainName)) ? (SingletonServiceBase<DomainCredentialsService>.Instance.DomainName + "\\" + SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser) : SingletonServiceBase<DomainCredentialsService>.Instance.DomainUser);
		}
		else
		{
			CurrentUser = ((!string.IsNullOrEmpty(Environment.UserDomainName)) ? (Environment.UserDomainName + "\\" + Environment.UserName) : Environment.UserName);
		}
	}

	private bool IsAtLeastOneIpSelected(string message)
	{
		if (!FilteredMachines.Any((Machine _) => _.IsSelected))
		{
			StatusMessage = message;
			return false;
		}
		return true;
	}

	private void ClearSelectedMachineStatuses()
	{
		foreach (Machine item in FilteredMachines.Where((Machine _) => _.IsSelected))
		{
			item.Status = null;
			item.State = MachineQueryState.None;
		}
	}

	private void OnKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
	{
		if (sender is TextBox { IsReadOnly: false } textBox && e.KeyboardDevice.IsKeyDown(Key.Tab))
		{
			textBox.SelectAll();
		}
	}

	private string GetStringResource(string resourceName)
	{
		return resourceManager.GetString(resourceName);
	}

	private void ResetSubnetFilter()
	{
		SubnetFilter = "*.*.*.*";
		FilterIpRange();
	}

	private void FilterIpRange(bool suppressStatusMessage = false)
	{
		try
		{
			IsLoading = true;
			Action<Machine> action = delegate(Machine it)
			{
				FilteredMachines.Add(it);
			};
			string resourceName = "DefaultSubnetFilterApplied";
			if (CanFilterIpRange())
			{
				resourceName = "SubnetFilterApplied";
				(IPAddress, IPAddress) ipAddressRange = GetIpAddressRange();
				IPAddress startIpAddressInclusive = ipAddressRange.Item1;
				IPAddress endIpAddressInclusive = ipAddressRange.Item2;
				action = delegate(Machine it)
				{
					if (IPAddress.Parse(it.Ip).IsInRange(startIpAddressInclusive, endIpAddressInclusive))
					{
						FilteredMachines.Add(it);
					}
					else
					{
						it.IsSelected = false;
					}
				};
			}
			FilteredMachines.Clear();
			foreach (Machine item in machineCollection)
			{
				action(item);
			}
			EnsureGlobalAddressCheckboxSynced();
			IsLoading = false;
			if (!suppressStatusMessage)
			{
				StatusMessage = GetStringResource(resourceName);
			}
		}
		catch (Exception ex)
		{
			IsLoading = false;
			if (!suppressStatusMessage)
			{
				StatusMessage = ex.Message;
			}
		}
	}

	private bool CanFilterIpRange()
	{
		var (iPAddress, iPAddress2) = GetIpAddressRange();
		if (iPAddress != null)
		{
			return iPAddress2 != null;
		}
		return false;
	}

	private (IPAddress startIpAddressInclusive, IPAddress endIpAddressInclusive) GetIpAddressRange()
	{
		try
		{
			if (string.IsNullOrEmpty(SubnetFilter))
			{
				return (startIpAddressInclusive: IPAddress.Parse("0.0.0.0"), endIpAddressInclusive: IPAddress.Parse("255.255.255.255"));
			}
			string[] array = SubnetFilter.Split('.');
			if (array.Length < 4)
			{
				List<string> list = new List<string>(array);
				while (list.Count < 4)
				{
					list.Add("*");
				}
				array = list.ToArray();
			}
			if (array.Length == 4)
			{
				byte[] array2 = new byte[array.Length];
				byte[] array3 = new byte[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					var (b, b2) = GetValidIpSegmentRange(array[i]);
					array2[i] = b;
					array3[i] = b2;
				}
				return (startIpAddressInclusive: new IPAddress(array2), endIpAddressInclusive: new IPAddress(array3));
			}
		}
		catch
		{
		}
		return (startIpAddressInclusive: null, endIpAddressInclusive: null);
	}

	private (byte startSegmentByteInclusive, byte endSegmentByteInclusive) GetValidIpSegmentRange(string segment)
	{
		if (string.IsNullOrEmpty(segment))
		{
			throw new InvalidOperationException("IP segments cannot be null or empty");
		}
		if (segment.Equals("*"))
		{
			return (startSegmentByteInclusive: 0, endSegmentByteInclusive: byte.MaxValue);
		}
		if (segment.Contains("-"))
		{
			string[] array = segment.Split('-');
			if (array.Length == 2 && byte.TryParse(array[0], out var result) && byte.TryParse(array[1], out var result2) && result <= result2)
			{
				return (startSegmentByteInclusive: result, endSegmentByteInclusive: result2);
			}
		}
		if (byte.TryParse(segment, out var result3))
		{
			return (startSegmentByteInclusive: result3, endSegmentByteInclusive: result3);
		}
		throw new ArgumentException("IP Segment not inside of the allowable range 0-255 or *");
	}
}
