using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.BLE.Abstractions.Utils;
using Plugin.BLE;
using System.Diagnostics;
using System.Collections.ObjectModel;
using GaiaDemo.Gaia;
using GaiaDemo.Gaia.Packet;
using GaiaDemo.Gaia.Requests;

namespace GaiaDemo
{
    public partial class MainPage : ContentPage
    {
        private readonly string TAG = "MainPage";

        readonly Guid UUID_GAIA_SERVICE = new Guid("00001100-d102-11e1-9b23-00025b00a5a5");
        readonly Guid UUID_GAIA_COMMAND_ENDPOINT = new Guid("00001101-d102-11e1-9b23-00025b00a5a5");
        readonly Guid UUID_GAIA_RESPONSE_ENDPOINT = new Guid("00001102-d102-11e1-9b23-00025b00a5a5");
        //readonly Guid UUID_GAIA_DATA_ENDPOINT = new Guid("00001103-d102-11e1-9b23-00025b00a5a5");

        private IAdapter mAdapter;
        private IBluetoothLE mBluetoothLE;
        private ICharacteristic EndpointCommand { get; set; }

        private ObservableCollection<SystemDeviceViewModel> systemDevices { get; set; }
        private ObservableCollection<ScanDeviceViewModel> scanDevices { get; set; }

        //private List<GaiaPacket> mQueue;

        private readonly byte[] PAYLOAD_BOOL_TRUE = { 0x01 };
        private readonly byte[] PAYLOAD_BOOL_FALSE = { 0x00 };

        string deviceName = "Unknow";
        bool isLedActivated = true;
        bool isEqBass = false;
        bool isEq3D = false;

        int[] features;
        int indexOfFeatures = 0;

        public MainPage()
        {
            InitializeComponent();

            InitFeatures();
            Init();
            GetSystemConnectedOrPairedDevices();
            ScanDevices();
        }

        private void InitFeatures()
        {
            features = new int[7];
            features[0] = GAIA.COMMAND_GET_API_VERSION;
            features[1] = GAIA.COMMAND_GET_LED_CONTROL;
            features[2] = GAIA.COMMAND_AV_REMOTE_CONTROL;
            features[3] = GAIA.COMMAND_GET_3D_ENHANCEMENT_CONTROL;
            features[4] = GAIA.COMMAND_GET_BASS_BOOST_CONTROL;
            features[5] = GAIA.COMMAND_GET_USER_EQ_CONTROL;
            features[6] = GAIA.COMMAND_GET_EQ_CONTROL;

            indexOfFeatures = 0;
        }

        private void Init()
        {
            RefreshToast("Bluetooth State: Unknow");
            RefreshLayout(false);

            mBluetoothLE = CrossBluetoothLE.Current;
            mAdapter = CrossBluetoothLE.Current.Adapter;

            mBluetoothLE.StateChanged += OnStateChanged;
            mAdapter.DeviceDiscovered += OnDeviceDiscovered;
            mAdapter.ScanTimeoutElapsed += OnScanTimeoutElapsed;
            mAdapter.DeviceDisconnected += OnDeviceDisconnected;
            mAdapter.DeviceConnectionLost += OnDeviceConnectionLost;

            RefreshToast("Bluetooth State: " + mBluetoothLE.State.ToString());
        }

        private void RefreshToast(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblToast.Text = message;
            });
        }

        private void RefreshLayout(bool isConnected)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (isConnected)
                {
                    btnScan.IsEnabled = false;
                    layoutDevices.IsVisible = false;
                    layoutDashboard.IsVisible = true;
                    lblInfo.IsVisible = true;
                    lblInfo.Text = deviceName;
                }
                else
                {
                    btnScan.IsEnabled = true;
                    layoutDevices.IsVisible = true;
                    layoutDashboard.IsVisible = false;
                    lblInfo.IsVisible = false;
                    btnLED.IsEnabled = false;
                }

                btnLED.IsVisible = false;

                layoutRemote.IsVisible = false;
                btnVMinus.IsEnabled = true;
                btnMute.IsEnabled = true;
                btnVPlus.IsEnabled = true;
                btnPlay.IsEnabled = true;
                btnPause.IsEnabled = true;
                btnStop.IsEnabled = true;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;

                layoutEQ.IsVisible = true;
                btnBass.IsEnabled = false;
                btn3D.IsEnabled = false;
                btnDefault.IsEnabled = false;
                btnClassic.IsEnabled = false;
                btnRock.IsEnabled = false;
                btnJazz.IsEnabled = false;
                btnFolk.IsEnabled = false;
                btnPop.IsEnabled = false;
            });
        }

        private void RefreshScanListView()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                btnScan.IsEnabled = true;
                lvScanDevices.ItemsSource = scanDevices;
                lvScanDevices.ItemTemplate = new DataTemplate(typeof(ScanDeviceCell));
            });
        }
        private void CheckDeviceFeatures()
        {
            if (indexOfFeatures <= features.Length - 1)
            {
                CreateRequest(CreatePacket(features[indexOfFeatures]));
                indexOfFeatures++;
            }
        }

        #region BT-General

        private void GetSystemConnectedOrPairedDevices()
        {
            if (mAdapter.GetSystemConnectedOrPairedDevices().Count == 0)
            {
                RefreshToast("Without System Connected Or Paired Devices");
                return;
            }

            systemDevices = new ObservableCollection<SystemDeviceViewModel>();
            try
            {
                SystemDeviceViewModel model;
                IDevice device;
                for (int i = 0; i < mAdapter.GetSystemConnectedOrPairedDevices().Count; i++)
                {
                    model = new SystemDeviceViewModel();
                    device = mAdapter.GetSystemConnectedOrPairedDevices().ElementAt(i);
                    model.Device = device;
                    model.Name = model.Device.Name;
                    systemDevices.Add(model);
                }
                lvSystemDevices.ItemsSource = systemDevices;
                lvSystemDevices.ItemTemplate = new DataTemplate(typeof(SystemDeviceCell));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(TAG + ":::GetSystemConnectedOrPairedDevices, " + ex.Message);
                RefreshToast("Get System Connected Or Paired Devices Failed");
            }
        }

        private void ScanDevices()
        {
            if (mBluetoothLE.State == BluetoothState.On)
            {
                lvScanDevices.ItemsSource = null;
                scanDevices = new ObservableCollection<ScanDeviceViewModel>();
                scanDevices.Clear();

                mAdapter.ScanTimeout = 5 * 1000;
                mAdapter.ScanMode = ScanMode.Balanced;
                mAdapter.StartScanningForDevicesAsync();
            }
            else
            {
                btnScan.IsEnabled = true;
                RefreshToast("Scan For Devices Failed");
            }
        }

        private async Task<bool> ConnectDevice(IDevice device)
        {
            await mAdapter.StopScanningForDevicesAsync();
            try
            {
                RefreshToast("Connecting " + device.Name);
                ConnectParameters connectParameters = new ConnectParameters(autoConnect: false, forceBleTransport: true);
                await mAdapter.ConnectToDeviceAsync(device, connectParameters);
                RefreshToast(device.Name + " Connected");

                _ = InitTargetDevice(device);
            }
            catch (DeviceConnectionException e)
            {
                Debug.WriteLine(TAG + ":::Connection Exception: " + e.Message);
                RefreshToast("Connect " + device.Name + " failed");
                return false;
            }

            return true;
        }

        private async Task InitTargetDevice(IDevice device)
        {
            var services = await device.GetServicesAsync();
            foreach (IService service in services)
            {
                if (service.Id.Equals(UUID_GAIA_SERVICE))
                {
                    RefreshToast("Discover GAIA Service");
                    var EndpointResponse = await service.GetCharacteristicAsync(UUID_GAIA_RESPONSE_ENDPOINT);
                    if (EndpointResponse != null)
                    {
                        EndpointResponse.ValueUpdated += OnCharacteristicUpdated;
                        await EndpointResponse.StartUpdatesAsync();

                        EndpointCommand = await service.GetCharacteristicAsync(UUID_GAIA_COMMAND_ENDPOINT);
                        if (EndpointCommand != null)
                        {
                            deviceName = device.Name;
                            RefreshLayout(true);

                            CheckDeviceFeatures();
                        }
                    }
                }
                else
                {
                    RefreshToast("Not Found GAIA Service");
                }
            }
        }

        #endregion

        #region BT-Write

        private void CreateRequest(GaiaPacket packet)
        {
            GaiaRequest request = new GaiaRequest(GaiaRequest.Type.SINGLE_REQUEST)
            {
                Packet = packet
            };
            ProcessRequest(request);
        }

        private GaiaPacket CreatePacket(int command)
        {
            return new GaiaPacketBLE(GAIA.VENDOR_QUALCOMM, command);
        }

        private GaiaPacket CreatePacket(int command, byte[] payload)
        {
            return new GaiaPacketBLE(GAIA.VENDOR_QUALCOMM, command, payload);
        }

        private void ProcessRequest(GaiaRequest request)
        {
            switch (request.Request)
            {
                case GaiaRequest.Type.SINGLE_REQUEST:
                    try
                    {
                        byte[] bytes = request.Packet.GetBytes();
                        SendGaiaCommandEndpoint(bytes);
                    }
                    catch (GaiaException e)
                    {
                        Debug.WriteLine(TAG + ":::Process Request Exception: " + e.Message);
                        RefreshToast("Process Request failed");
                    }
                    break;
                case GaiaRequest.Type.ACKNOWLEDGEMENT:
                    break;
                case GaiaRequest.Type.UNACKNOWLEDGED_REQUEST:
                    break;
            }
        }

        private async void SendGaiaCommandEndpoint(byte[] data)
        {
            string value = Utils.GetHexStringFromBytes(data);
            try
            {
                bool result = await EndpointCommand.WriteAsync(data);
                if (result)
                {
                    Debug.WriteLine(TAG + ":::REQ: " + value);
                }
                else
                {
                    RefreshToast("Send Failed: " + value);
                    Debug.WriteLine(TAG + ":::Send Failed: " + value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(TAG + ":::Send Exception: " + ex.Message);
            }
        }

        #endregion

        #region BT-Notifacation

        private void OnCharacteristicUpdated(object sender, CharacteristicUpdatedEventArgs args)
        {
            ICharacteristic characteristic = args.Characteristic;
            if (characteristic != null)
            {
                Guid uuid = characteristic.Id;
                if (uuid == UUID_GAIA_RESPONSE_ENDPOINT)
                {
                    byte[] bytes = characteristic.Value;
                    if (bytes != null)
                    {
                        string value = Utils.GetHexStringFromBytes(bytes);
                        Debug.WriteLine(TAG + ":::RSP: " + value);
                        value = DateTime.Now.ToString("HH:mm:ss") + " Received: " + value;
                        RefreshToast(value);

                        ReceiveGaiaPacket(bytes);
                    }
                }
            }
        }

        #region Receive

        private void ReceiveGaiaPacket(byte[] data)
        {
            GaiaPacket packet = new GaiaPacketBLE(data);
            if (packet.IsAcknowledgement())
            {
                CheckDeviceFeatures();

                int command = packet.GetCommand();
                switch (command)
                {
                    case GAIA.COMMAND_GET_API_VERSION:
                        ReceivePacketGetAPIVersionACK(packet);
                        break;
                    case GAIA.COMMAND_GET_LED_CONTROL:
                        ReceivePacketGetLedControlACK(packet);
                        break;
                    case GAIA.COMMAND_AV_REMOTE_CONTROL:
                        ReceivePacketGetRemoteControlACK(packet);
                        break;
                    case GAIA.COMMAND_GET_3D_ENHANCEMENT_CONTROL:
                    case GAIA.COMMAND_SET_3D_ENHANCEMENT_CONTROL:
                        ReceivePacketGetUserEqualizerControlACK(EqualizerControls.ENHANCEMENT_3D, packet);
                        break;
                    case GAIA.COMMAND_GET_BASS_BOOST_CONTROL:
                    case GAIA.COMMAND_SET_BASS_BOOST_CONTROL:
                        ReceivePacketGetUserEqualizerControlACK(EqualizerControls.BASS_BOOST, packet);
                        break;
                    case GAIA.COMMAND_GET_USER_EQ_CONTROL:
                    case GAIA.COMMAND_SET_USER_EQ_CONTROL:
                        ReceivePacketGetUserEqualizerControlACK(EqualizerControls.USER, packet);
                        break;
                    case GAIA.COMMAND_GET_EQ_CONTROL:
                    case GAIA.COMMAND_SET_EQ_CONTROL:
                        ReceivePacketGetEqualizerControlACK(packet);
                        break;
                }                
            }
            else
            {
                string value = "Packet has not been managed by application, manager sends NOT_SUPPORTED acknowledgement," +
                    " bytes: \n\t\t" + GaiaUtils.GetCommandToString(packet.GetCommandId());
                RefreshToast(value);
            }
        }

        private void ReceivePacketGetAPIVersionACK(GaiaPacket packet)
        {
            GAIA.Status status = packet.GetStatus();
            if (status == GAIA.Status.SUCCESS)
            {
                byte[] payload = packet.GetPayload();
                int PAYLOAD_VALUE_1_OFFSET = 1;
                int PAYLOAD_VALUE_2_OFFSET = PAYLOAD_VALUE_1_OFFSET + 1;
                int PAYLOAD_VALUE_3_OFFSET = PAYLOAD_VALUE_2_OFFSET + 1;
                int PAYLOAD_VALUE_LENGTH = 3;
                int PAYLOAD_MIN_LENGTH = PAYLOAD_VALUE_LENGTH + 1; // ACK status length is 1

                if (payload.Length >= PAYLOAD_MIN_LENGTH)
                {
                    UpdateLblInfo(payload[PAYLOAD_VALUE_1_OFFSET],
                            payload[PAYLOAD_VALUE_2_OFFSET],
                            payload[PAYLOAD_VALUE_3_OFFSET]);
                }
            }
            else
            {
                UpdateLblInfo(0, 0, 0);
            }
        }

        private void ReceivePacketGetLedControlACK(GaiaPacket packet)
        {
            GAIA.Status status = packet.GetStatus();
            if (status == GAIA.Status.SUCCESS)
            {
                byte[] payload = packet.GetPayload();
                int PAYLOAD_VALUE_OFFSET = 1;
                int PAYLOAD_VALUE_LENGTH = 1;
                int PAYLOAD_MIN_LENGTH = PAYLOAD_VALUE_LENGTH + 1; // ACK status length is 1

                if (payload.Length >= PAYLOAD_MIN_LENGTH)
                {
                    isLedActivated = payload[PAYLOAD_VALUE_OFFSET] == 0x01;
                    UpdateBtnLED(true);
                }
            }
            else
            {
                isLedActivated = false;
                UpdateBtnLED(false);
            }            
        }

        private void ReceivePacketGetRemoteControlACK(GaiaPacket packet)
        {
            if (indexOfFeatures <= features.Length)
            {
                GAIA.Status status = packet.GetStatus();
                if (status != GAIA.Status.NOT_SUPPORTED)
                {
                    UpdateLayoutRemote();
                }
            }
            else
            {
                GAIA.Status status = packet.GetStatus();
                if (status != GAIA.Status.SUCCESS)
                {
                    RefreshToast("The remote control feature is not supported.");
                }
            }
        }

        private void ReceivePacketGetUserEqualizerControlACK(EqualizerControls control, GaiaPacket packet)
        {
            if (indexOfFeatures <= features.Length - 1)
            {
                GAIA.Status status = packet.GetStatus();
                if (status != GAIA.Status.NOT_SUPPORTED)
                {
                    UpdateLayoutEQ();
                }
            }
            else
            {
                GAIA.Status status = packet.GetStatus();
                if (status == GAIA.Status.SUCCESS)
                {
                    //byte[] payload = packet.GetPayload();
                    //int PAYLOAD_VALUE_OFFSET = 1;
                    //int PAYLOAD_VALUE_LENGTH = 1;
                    //int PAYLOAD_MIN_LENGTH = PAYLOAD_VALUE_LENGTH + 1; // ACK status length is 1

                    //if (payload.Length >= PAYLOAD_MIN_LENGTH)
                    //{
                    //    bool activate = payload[PAYLOAD_VALUE_OFFSET] == 0x01;
                    //    UpdateEqalizerButtons(control, activate);
                    //}
                }
                else
                {
                    //UpdateEqalizerButtons(control, false);
                    RefreshToast("User Equalizer not supported.");
                }
            }
        }

        private void ReceivePacketGetEqualizerControlACK(GaiaPacket packet)
        {
            GAIA.Status status = packet.GetStatus();
            if (status == GAIA.Status.SUCCESS)
            {
                //byte[] payload = packet.GetPayload();
                //int PAYLOAD_VALUE_OFFSET = 1;
                //int PAYLOAD_VALUE_LENGTH = 1;
                //int PAYLOAD_MIN_LENGTH = PAYLOAD_VALUE_LENGTH + 1; // ACK status length is 1

                //if (payload.Length >= PAYLOAD_MIN_LENGTH)
                //{
                //    int preset = payload[PAYLOAD_VALUE_OFFSET];
                //    UpdateEqalizerButtons((EqualizerPreset)preset);
                //}
            }
            else
            {
                RefreshToast("User Equalizer not supported.");
            }
        }

        #endregion

        #endregion

        #region Information

        private void UpdateLblInfo(int part1, int part2, int part3)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                string api = string.Format(", API: {0}.{1}.{2}", part1, part2, part3);
                lblInfo.Text = deviceName + api;
            });

            //CheckDeviceFeatures();
        }

        private void UpdateBtnLED(bool isEnable)
        {
            if (isEnable)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    btnLED.IsVisible = true;
                    btnLED.IsEnabled = true;
                    if (isLedActivated)
                    {
                        btnLED.Text = "LED - ON";
                    }
                    else
                    {
                        btnLED.Text = "LED - OFF";
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    btnLED.IsVisible = false;
                    btnLED.IsEnabled = false;
                    btnLED.Text = "LED";
                });
            }            
        }

        private void UpdateBtnLED()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                btnLED.IsVisible = true;
                btnLED.IsEnabled = true;
                if (isLedActivated)
                {
                    btnLED.Text = "LED - ON";
                }
                else
                {
                    btnLED.Text = "LED - OFF";
                }
            });
        }

        private void OnLedClicked(object sender, EventArgs e)
        {
            isLedActivated = !isLedActivated;
            byte[] payload = isLedActivated ? PAYLOAD_BOOL_TRUE : PAYLOAD_BOOL_FALSE;
            GaiaPacket packet = CreatePacket(GAIA.COMMAND_SET_LED_CONTROL, payload);
            CreateRequest(packet);

            UpdateBtnLED();
        }

        #endregion

        #region Remote Control
        private void UpdateLayoutRemote()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                layoutRemote.IsVisible = true;
            });

            //CheckDeviceFeatures();
        }

        private void SendRemote(int control)
        {
            int PAYLOAD_LENGTH = 1;
            int CONTROL_OFFSET = 0;
            byte[] payload = new byte[PAYLOAD_LENGTH];
            payload[CONTROL_OFFSET] = (byte)control;
            CreateRequest(CreatePacket(GAIA.COMMAND_AV_REMOTE_CONTROL, payload));
        }

        private void OnVolumePlusClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.VOLUME_UP);
        }

        private void OnVolumeMinusClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.VOLUME_DOWN);
        }

        private void OnMuteClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.MUTE);
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.PLAY);
        }

        private void OnPauseClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.PAUSE);
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.STOP);
        }

        private void OnPreviousClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.REWIND);
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            SendRemote((int)RemoteControls.FORWARD);
        }

        #endregion

        #region Equalizer

        private void UpdateLayoutEQ()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                btnBass.IsEnabled = true;
                btn3D.IsEnabled = true;
                btnDefault.IsEnabled = true;
                btnClassic.IsEnabled = true;
                btnRock.IsEnabled = true;
                btnJazz.IsEnabled = true;
                btnFolk.IsEnabled = true;
                btnPop.IsEnabled = true;
            });
        }

        private void SetPreset(EqualizerPreset preset)
        {
            int PAYLOAD_LENGTH = 1;
            int PRESET_OFFSET = 0;
            byte[] payload = new byte[PAYLOAD_LENGTH];
            payload[PRESET_OFFSET] = (byte)preset;
            CreateRequest(CreatePacket(GAIA.COMMAND_SET_EQ_CONTROL, payload));
        }

        private void UpdateEqalizerButtons(EqualizerControls control, bool activate)
        {
            switch (control)
            {
                case EqualizerControls.ENHANCEMENT_3D:
                    isEq3D = activate;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        btn3D.IsEnabled = true;
                        if (isEq3D)
                        {
                            btn3D.Text = "3D - ON";
                        }
                        else
                        {
                            btn3D.Text = "3D - OFF";
                        }
                    });
                    break;
                case EqualizerControls.BASS_BOOST:
                    isEqBass = activate;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        btn3D.IsEnabled = true;
                        if (isEqBass)
                        {
                            btn3D.Text = "Bass boost - ON";
                        }
                        else
                        {
                            btn3D.Text = "Bass boost - OFF";
                        }
                    });
                    break;
                case EqualizerControls.USER:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();
                    });
                    break;
            }
        }

        private void InitEqulizeralPresetButtons()
        {
            btnDefault.Text = "Default";
            btnRock.Text = "Rock";
            btnJazz.Text = "Jazz";
            btnFolk.Text = "Folk";
            btnPop.Text = "Pop";
            btnClassic.Text = "Classic";
        }

        private void UpdateEqalizerButtons(EqualizerPreset preset)
        {
            switch (preset)
            {
                case EqualizerPreset.Default:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();

                        btnDefault.IsEnabled = true;
                        btnDefault.Text = "Default - ON";
                    });
                    break;
                case EqualizerPreset.Rock:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();

                        btnRock.IsEnabled = true;
                        btnRock.Text = "Rock - ON";
                    });
                    break;
                case EqualizerPreset.Jazz:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();

                        btnJazz.IsEnabled = true;
                        btnJazz.Text = "Jazz - ON";
                    });
                    break;
                case EqualizerPreset.Folk:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();

                        btnFolk.IsEnabled = true;
                        btnFolk.Text = "Folk - ON";
                    });
                    break;
                case EqualizerPreset.Pop:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();

                        btnPop.IsEnabled = true;
                        btnPop.Text = "Pop - ON";
                    });
                    break;
                case EqualizerPreset.Classic:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitEqulizeralPresetButtons();

                        btnClassic.IsEnabled = true;
                        btnClassic.Text = "Classic - ON";
                    });
                    break;
            }
        }

        private void OnBassBoostClicked(object sender, EventArgs e)
        {
            isEqBass = !isEqBass;
            byte[] payload = isEqBass ? PAYLOAD_BOOL_TRUE : PAYLOAD_BOOL_FALSE;
            CreateRequest(CreatePacket(GAIA.COMMAND_SET_BASS_BOOST_CONTROL, payload));
        }

        private void On3DClicked(object sender, EventArgs e)
        {
            isEq3D = !isEq3D;
            byte[] payload = isEq3D ? PAYLOAD_BOOL_TRUE : PAYLOAD_BOOL_FALSE;
            CreateRequest(CreatePacket(GAIA.COMMAND_SET_3D_ENHANCEMENT_CONTROL, payload));
        }

        private void OnDefaultClicked(object sender, EventArgs e)
        {
            SetPreset(EqualizerPreset.Default);
        }

        private void OnClassicClicked(object sender, EventArgs e)
        {
            SetPreset(EqualizerPreset.Classic);
        }

        private void OnRockClicked(object sender, EventArgs e)
        {
            SetPreset(EqualizerPreset.Rock);
        }

        private void OnJazzClicked(object sender, EventArgs e)
        {
            SetPreset(EqualizerPreset.Jazz);
        }

        private void OnFolkClicked(object sender, EventArgs e)
        {
            SetPreset(EqualizerPreset.Folk);
        }

        private void OnPopClicked(object sender, EventArgs e)
        {
            SetPreset(EqualizerPreset.Pop);
        }

        #endregion

        #region Events

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            btnScan.IsEnabled = e.NewState == BluetoothState.On;
            RefreshToast("Bluetooth State: " + e.NewState.ToString());
        }

        private void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            RefreshLayout(false);
            RefreshToast("Device Disconnected");
        }

        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            RefreshLayout(false);
            RefreshToast("Device Connection Lost");
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            IDevice device = e.Device;
            var vm = scanDevices.FirstOrDefault(d => d.Device.Id == device.Id);
            if (vm == null)
            {
                ScanDeviceViewModel model = new ScanDeviceViewModel
                {
                    Device = device,
                    Name = device.Name ?? "Unknow"
                };
                scanDevices.Add(model);
            }
        }

        private void OnScanTimeoutElapsed(object sender, EventArgs e)
        {
            RefreshScanListView();
        }

        private void OnScanClicked(object sender, EventArgs e)
        {
            ScanDevices();
        }

        private void OnScanDevicesItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ScanDeviceViewModel model = (ScanDeviceViewModel)e.SelectedItem;
            IDevice device = model.Device;
            _ = ConnectDevice(device);
        }

        #endregion

        #region Enum

        private enum RemoteControls
        {
            /// <summary>
            /// The AV remote control operation for volume up.
            /// </summary>
            VOLUME_UP = 0x41,

            /// <summary>
            /// The AV remote control operation for volume down.
            /// </summary>
            VOLUME_DOWN = 0x42,

            /// <summary>
            /// The AV remote control operation for mute.
            /// </summary>
            MUTE = 0x43,

            /// <summary>
            /// The AV remote control operation for play.
            /// </summary>
            PLAY = 0x44,

            /// <summary>
            /// The AV remote control operation for stop.
            /// </summary>
            STOP = 0x45,

            /// <summary>
            /// The AV remote control operation for pause.
            /// </summary>
            PAUSE = 0x46,

            /// <summary>
            /// The AV remote control operation for reward.
            /// </summary>
            REWIND = 0x4C,

            /// <summary>
            /// The AV remote control operation for forward.
            /// </summary>
            FORWARD = 0x4B
        }

        /// <summary>
        /// This enumeration regroups all the different controls which corresponds to the Equalizer feature.
        /// </summary>
        private enum EqualizerControls
        {
            /// <summary>
            /// control the 3D enhancement
            /// GAIA.COMMAND_GET_3D_ENHANCEMENT_CONTROL: to get the current activation state (enabled/disabled).
            /// GAIA.COMMAND_SET_3D_ENHANCEMENT_CONTROL: to set up the activation state.
            /// </summary>
            ENHANCEMENT_3D = 1,

            /// <summary>
            /// control the Boost bass
            /// GAIA.COMMAND_GET_BASS_BOOST_CONTROL: to get the current activation state (enabled/disabled).
            /// GAIA.COMMAND_SET_BASS_BOOST_CONTROL: to set up the activation state.
            /// </summary>
            BASS_BOOST = 2,

            /// <summary>
            /// control the pre-set banks
            /// GAIA.COMMAND_GET_USER_EQ_CONTROL: to get the current activation state of the pre-sets (enabled/disabled)
            /// GAIA.COMMAND_SET_USER_EQ_CONTROL: to set up the activation state.
            /// GAIA.COMMAND_GET_EQ_CONTROL: to get the current pre-set.
            /// GAIA.COMMAND_SET_EQ_CONTROL: to set up the selected pre-set.
            /// </summary>
            USER = 3
        }

        private enum EqualizerPreset
        {
            Default = 0,

            Custom = 1,

            Rock = 2,

            Jazz = 3,

            Folk = 4,

            Pop = 5,

            Classic = 6
        }

        #endregion

        
    }
}
