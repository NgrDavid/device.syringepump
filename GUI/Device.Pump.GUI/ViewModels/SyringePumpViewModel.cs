#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Themes.Fluent;
using Bonsai.Harp;
using Device.Pump.GUI.Models;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Serilog;
using OperatingSystem = System.OperatingSystem;

#endregion

namespace Device.Pump.GUI.ViewModels
{
    public class SyringePumpViewModel : ReactiveValidationObject
    {
        public string AppVersion { get; set; }

        [Reactive] public List<string> Ports { get; set; }

        [Reactive] public string SelectedPort { get; set; }
        [Reactive] public bool Connected { get; set; }

        [Reactive] public ObservableCollection<string> HarpMessages { get; set; }

        [Reactive] public bool StepStateEvent { get; set; } = true;
        [Reactive] public bool DirectionStateEvent { get; set; } = true;
        [Reactive] public bool SwitchForwardStateEvent { get; set; } = true;
        [Reactive] public bool SwitchReverseStateEvent { get; set; } = true;
        [Reactive] public bool InputStateEvent { get; set; } = true;
        [Reactive] public bool ProtocolStateEvent { get; set; } = true;

        [Reactive] public int ProtocolType { get; set; }

        [Reactive] public string DeviceName { get; set; }
        [Reactive] public int DeviceID { get; set; }
        [Reactive] public HarpVersion HardwareVersion { get; set; }
        [Reactive] public HarpVersion FirmwareVersion { get; set; }

        [Reactive] public int NumberOfSteps { get; set; } = 15;
        [Reactive] public int StepPeriod { get; set; } = 10;
        [Reactive] public float Flowrate { get; set; } = 0.5f;
        [Reactive] public float Volume { get; set; } = 0.5f;
        [Reactive] public int MotorMicrostep { get; set; }
        [Reactive] public int DigitalInput0Config { get; set; }
        [Reactive] public int DigitalOutput0Config { get; set; }
        [Reactive] public int DigitalOutput1Config { get; set; }
        [Reactive] public int CalibrationValue1 { get; set; }
        [Reactive] public int CalibrationValue2 { get; set; }
        [Reactive] public Direction ProtocolDirection { get; set; }

        [Reactive] public List<Direction> Directions { get; set; }

        [Reactive] public bool ShowLogs { get; set; } = false;

        [ObservableAsProperty] public bool IsLoadingPorts { get; }

        [ObservableAsProperty] public bool IsConnecting { get; }

        [ObservableAsProperty] public bool IsResetting { get; }

        [ObservableAsProperty] public bool IsSaving { get; }

        [ObservableAsProperty] public bool IsRunningProtocol { get; }

        [Reactive] public bool ShowDarkTheme { get; set; }

        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }
        public ReactiveCommand<Unit, Unit> StartProtocolCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowLogsCommand { get; }
        public ReactiveCommand<bool, Unit> SaveConfigurationCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetConfigurationCommand { get; }

        public ReactiveCommand<Unit, Unit> ChangeThemeCommand { get; }

        private Bonsai.Harp.Device _dev;
        private readonly IObserver<HarpMessage> _observer;
        private IDisposable _observable;
        private readonly Subject<HarpMessage> _msgsSubject;
        private DeviceConfiguration configuration;

        public SyringePumpViewModel()
        {
            var assembly = typeof(SyringePumpViewModel).Assembly;
            var informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            AppVersion = $"v{informationVersion}";

            Console.WriteLine(
                $"Dotnet version: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");

            HarpMessages = new ObservableCollection<string>();
            Directions = Enum.GetValues<Direction>().ToList();

            LoadDeviceInformation = ReactiveCommand.CreateFromObservable(LoadUsbInformation);
            LoadDeviceInformation.IsExecuting.ToPropertyEx(this, x => x.IsLoadingPorts);
            LoadDeviceInformation.ThrownExceptions.Subscribe(ex =>
                Log.Error(ex, "Error loading device information with exception: {Exception}", ex));

            // can connect if there is a selection and also if the new selection is different than the old one
            var canConnect = this.WhenAnyValue(x => x.SelectedPort)
                .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

            ConnectAndGetBaseInfoCommand = ReactiveCommand.CreateFromObservable(ConnectAndGetBaseInfo, canConnect);
            ConnectAndGetBaseInfoCommand.IsExecuting.ToPropertyEx(this, x => x.IsConnecting);
            ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex =>
                Log.Error(ex, "Error connecting to device with error: {Exception}", ex));

            var canChangeConfig = this.WhenAnyValue(x => x.Connected).Select(connected => connected);
            StartProtocolCommand = ReactiveCommand.CreateFromObservable(StartProtocol, canChangeConfig);
            StartProtocolCommand.IsExecuting.ToPropertyEx(this, x => x.IsRunningProtocol);
            StartProtocolCommand.ThrownExceptions.Subscribe(ex =>
                Log.Error(ex, "Error starting protocol with error: {Exception}", ex));

            ShowLogsCommand = ReactiveCommand.Create(() => { ShowLogs = !ShowLogs; }, canChangeConfig);

            SaveConfigurationCommand =
                ReactiveCommand.CreateFromObservable<bool, Unit>(SaveConfiguration, canChangeConfig);
            SaveConfigurationCommand.IsExecuting.ToPropertyEx(this, x => x.IsSaving);
            SaveConfigurationCommand.ThrownExceptions.Subscribe(ex =>
                Log.Error(ex, "Error saving configuration with error: {Exception}", ex));

            ResetConfigurationCommand = ReactiveCommand.CreateFromObservable(ResetConfiguration, canChangeConfig);
            ResetConfigurationCommand.IsExecuting.ToPropertyEx(this, x => x.IsResetting);
            ResetConfigurationCommand.ThrownExceptions.Subscribe(ex =>
                Log.Error(ex, "Error resetting device configuration with error: {Exception}", ex));

            ChangeThemeCommand = ReactiveCommand.Create(ChangeTheme);

            // TODO: missing properly dispose of this
            _msgsSubject = new Subject<HarpMessage>();

            _observer = Observer.Create<HarpMessage>(item => HarpMessages.Add(item.ToString()),
                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); },
                () => HarpMessages.Add("Completed sending commands to device"));

            // Validation rules
            this.ValidationRule(viewModel => viewModel.NumberOfSteps,
                steps => steps > 0,
                "Number of steps only accepts integer values greater than 0");
            this.ValidationRule(viewModel => viewModel.StepPeriod,
                period => period > 0,
                "Step Period only accepts integer values greater than 0");
            this.ValidationRule(viewModel => viewModel.Flowrate,
                flowRate => flowRate > 0,
                "Flowrate only accepts float values greater than 0");
            this.ValidationRule(viewModel => viewModel.Volume,
                volume => volume > 0,
                "Volume only accepts float values greater than 0");

            // force initial population of currently connected ports
            LoadUsbInformation();
        }

        private void ChangeTheme()
        {
            Application.Current.Styles[0] = new FluentTheme(new Uri("avares://ControlCatalog/Styles"))
            {
                Mode = ShowDarkTheme ? FluentThemeMode.Dark : FluentThemeMode.Light
            };
        }

        private IObservable<Unit> LoadUsbInformation()
        {
            return Observable.Start(() =>
            {
                var devices = SerialPort.GetPortNames();

                if (OperatingSystem.IsMacOS())
                    Ports = devices.Where(d => d.Contains("cu.")).ToList();
                else
                    Ports = devices.ToList();

                Log.Information("Loaded USB information");
            });
        }

        private IObservable<Unit> StartProtocol()
        {
            return Observable.StartAsync(async () =>
            {
                var startProtocolMessage = HarpCommand.WriteByte((int)PumpRegisters.StartProtocol, 1);

                _msgsSubject.OnNext(startProtocolMessage);

                await Task.Delay(200);

                Log.Information("Started protocol");
            });
        }

        private IObservable<Unit> SaveConfiguration(bool savePermanently)
        {
            return Observable.StartAsync(async () =>
            {
                if (_dev == null)
                    throw new Exception("You need to connect to the device first");

                var msgs = new List<HarpMessage>();

                // send commands to save every config (verify each step for board's reply)

                // events
                byte events = (byte)((Convert.ToByte(StepStateEvent) << 0) |
                                     (Convert.ToByte(DirectionStateEvent) << 1) |
                                     (Convert.ToByte(SwitchForwardStateEvent) << 2) |
                                     (Convert.ToByte(SwitchReverseStateEvent) << 3) |
                                     (Convert.ToByte(InputStateEvent) << 4) |
                                     (Convert.ToByte(ProtocolStateEvent) << 5));
                var eventsMessage = HarpCommand.WriteByte((int)PumpRegisters.EventsEnable, events);
                msgs.Add(eventsMessage);

                // motor microstep
                byte motor = Convert.ToByte(MotorMicrostep);
                var motorMessage = HarpCommand.WriteByte((int)PumpRegisters.MotorMicrostep, motor);
                msgs.Add(motorMessage);

                // di0, do0 and do1
                byte di0 = Convert.ToByte(DigitalInput0Config);
                var di0Message = HarpCommand.WriteByte((int)PumpRegisters.DigitalInput0Config, di0);
                msgs.Add(di0Message);

                byte do0 = Convert.ToByte(DigitalOutput0Config);
                var do0Message = HarpCommand.WriteByte((int)PumpRegisters.DigitalOutput0Config, do0);
                msgs.Add(do0Message);

                byte do1 = Convert.ToByte(DigitalOutput1Config);
                var do1Message = HarpCommand.WriteByte((int)PumpRegisters.DigitalOutput1Config, do1);
                msgs.Add(do1Message);

                // protocol
                // protocol type
                byte protocolType = Convert.ToByte(ProtocolType);
                var protocolTypeMessage = HarpCommand.WriteByte((int)PumpRegisters.ProtocolType, protocolType);
                msgs.Add(protocolTypeMessage);

                // protocol direction
                byte protocolDirection = Convert.ToByte(ProtocolDirection);
                var protocolDirectionMessage =
                    HarpCommand.WriteByte((int)PumpRegisters.ProtocolDirection, protocolDirection);
                msgs.Add(protocolDirectionMessage);

                // if step:
                if (protocolType == 0)
                {
                    // number of steps
                    ushort numberOfSteps = Convert.ToUInt16(NumberOfSteps);
                    var numberOfStepsMessage =
                        HarpCommand.WriteUInt16((int)PumpRegisters.ProtocolNumberOfSteps, numberOfSteps);
                    msgs.Add(numberOfStepsMessage);

                    // step period
                    ushort stepPeriod = Convert.ToUInt16(StepPeriod);
                    var stepPeriodMessage = HarpCommand.WriteUInt16((int)PumpRegisters.ProtocolStepPeriod, stepPeriod);
                    msgs.Add(stepPeriodMessage);
                }
                else
                {
                    // flowrate
                    float flowrate = Convert.ToSingle(Flowrate);
                    var flowrateMessage = HarpCommand.WriteSingle((int)PumpRegisters.ProtocolFlowrate, flowrate);
                    msgs.Add(flowrateMessage);
                    // volume
                    float volume = Convert.ToSingle(Volume);
                    var volumeMessage = HarpCommand.WriteSingle((int)PumpRegisters.ProtocolVolume, volume);
                    msgs.Add(volumeMessage);

                    // calibration val 1
                    byte calValue1 = Convert.ToByte(CalibrationValue1);
                    var calValue1Message = HarpCommand.WriteByte((int)PumpRegisters.CalibrationValue1, calValue1);
                    msgs.Add(calValue1Message);

                    // calibration val 2
                    byte calValue2 = Convert.ToByte(CalibrationValue2);
                    var calValue2Message = HarpCommand.WriteByte((int)PumpRegisters.CalibrationValue2, calValue2);
                    msgs.Add(calValue2Message);
                }

                if (savePermanently)
                {
                    var resetMessage = HarpCommand.ResetDevice(ResetMode.Save);
                    msgs.Add(resetMessage);
                }

                // send all messages independently of the save type
                HarpMessages.Clear();

                foreach (var harpMessage in msgs)
                {
                    _msgsSubject.OnNext(harpMessage);
                }

                await Task.Delay(500);
            });
        }

        private IObservable<Unit> ResetConfiguration()
        {
            return Observable.StartAsync(async () =>
            {
                var resetMessage = HarpCommand.ResetDevice(ResetMode.RestoreDefault);

                _msgsSubject.OnNext(resetMessage);

                await Task.Delay(2000);

                await ConnectAndGetBaseInfo();

                // send message to opened device
                // //TODO: when we have the observable from the receiving data, we should present a message stating if the operation completed successfully
            });
        }

        private IObservable<Unit> ConnectAndGetBaseInfo()
        {
            return Observable.StartAsync(async () =>
            {
                if (string.IsNullOrEmpty(SelectedPort))
                    throw new Exception("invalid parameter");

                configuration = new DeviceConfiguration();
                if (_dev != null)
                {
                    // cleanup variables
                    _observable?.Dispose();
                    _observable = null;
                }

                _dev = new Bonsai.Harp.Device
                {
                    PortName = SelectedPort,
                    Heartbeat = EnableType.Disable,
                    IgnoreErrors = false
                };

                Log.Information("Attempting connection to port \'{SelectedPort}\'", SelectedPort);

                HarpMessages.Clear();

                await Task.Delay(300);

                var observable = _dev.Generate()
                    .Where(MessageType.Read)
                    .Do(ReadRegister)
                    .Throttle(TimeSpan.FromSeconds(0.2))
                    .Timeout(TimeSpan.FromSeconds(5))
                    .Subscribe(_ => { },
                                // FIXME: ignore here the connection and perhaps simply return?
                                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); });

                await Task.Delay(300);

                Log.Information("Connection established with the following return information: {Info}", configuration);

                // present messagebox if we are not handling a Pump device
                if (configuration.WhoAmI != 1280)
                {
                    // when the configuration.WhoAmI is zero, we are dealing with a non-HARP device, so change message accordingly
                    var message = $"Found a HARP device: {configuration.DeviceName} ({configuration.WhoAmI}).\n\nThis GUI is only for the SyringePump HARP device.\n\nPlease select another serial port.";
                    var icon = Icon.Info;
                    if (configuration.WhoAmI == 0)
                    {
                        message =
                            $"Found a non-HARP device.\n\nThis GUI is only for the SyringePump HARP device.\n\nPlease select another serial port.";
                        icon = Icon.Error;
                    }

                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Unexpected device found",
                            message,
                            icon: icon);
                    await messageBoxStandardWindow.Show();
                    observable.Dispose();
                    return;
                }

                DeviceName = configuration.DeviceName;
                DeviceID = configuration.WhoAmI;

                // convert Hw and Fw version
                HardwareVersion = configuration.HardwareVersion;
                FirmwareVersion = configuration.FirmwareVersion;

                Connected = true;

                observable.Dispose();

                // generate observable for remaining operations
                _observable = _dev.Generate(_msgsSubject)
                    .Subscribe(_observer);
            });
        }

        private void ReadRegister(HarpMessage message)
        {
            switch (message.Address)
            {
                case DeviceRegisters.WhoAmI:
                    configuration.WhoAmI = message.GetPayloadUInt16();
                    break;
                case DeviceRegisters.HardwareVersionHigh:
                    configuration.HardwareVersionHigh = message.GetPayloadByte();
                    break;
                case DeviceRegisters.HardwareVersionLow:
                    configuration.HardwareVersionLow = message.GetPayloadByte();
                    break;
                case DeviceRegisters.FirmwareVersionHigh:
                    configuration.FirmwareVersionHigh = message.GetPayloadByte();
                    break;
                case DeviceRegisters.FirmwareVersionLow:
                    configuration.FirmwareVersionLow = message.GetPayloadByte();
                    break;
                case DeviceRegisters.CoreVersionHigh:
                    configuration.CoreVersionHigh = message.GetPayloadByte();
                    break;
                case DeviceRegisters.CoreVersionLow:
                    configuration.CoreVersionLow = message.GetPayloadByte();
                    break;
                case DeviceRegisters.AssemblyVersion:
                    configuration.AssemblyVersion = message.GetPayloadByte();
                    break;
                case DeviceRegisters.TimestampSecond:
                    configuration.Timestamp = message.GetPayloadUInt32();
                    break;
                case DeviceRegisters.DeviceName:
                    var deviceName = nameof(Device);
                    if (!message.Error)
                    {
                        var namePayload = message.GetPayload();
                        deviceName = Encoding.ASCII.GetString(namePayload.Array, namePayload.Offset, namePayload.Count)
                            .Trim('\0');
                    }

                    configuration.DeviceName = deviceName;
                    break;
                case DeviceRegisters.SerialNumber:
                    configuration.SerialNumber = message.GetPayloadUInt16();
                    break;
            }

            // Update UI with the remaining registers
            if (message.Address >= (int)(PumpRegisters.EnableMotorDriver))
                UpdateUI(message);
        }

        private void UpdateUI(HarpMessage item)
        {
            switch ((PumpRegisters)item.Address)
            {
                case PumpRegisters.EnableMotorDriver:
                case PumpRegisters.StartProtocol:
                case PumpRegisters.StepState:
                case PumpRegisters.DirState:
                case PumpRegisters.SwitchForwardState:
                case PumpRegisters.SwitchReverseState:
                case PumpRegisters.InputState:
                    break;
                case PumpRegisters.SetDigitalOutputs:
                    //TODO: add this element to the UI
                    break;
                case PumpRegisters.ClearDigitalOutputs:
                    //TODO: add this element to the UI
                    break;
                case PumpRegisters.DigitalOutput0Config:
                    DigitalOutput0Config = item.GetPayloadByte();
                    break;
                case PumpRegisters.DigitalOutput1Config:
                    DigitalOutput1Config = item.GetPayloadByte();
                    break;
                case PumpRegisters.DigitalInput0Config:
                    DigitalInput0Config = item.GetPayloadByte();
                    break;
                case PumpRegisters.MotorMicrostep:
                    MotorMicrostep = item.GetPayloadByte();
                    break;
                case PumpRegisters.ProtocolNumberOfSteps:
                    NumberOfSteps = item.GetPayloadUInt16();
                    break;
                case PumpRegisters.ProtocolFlowrate:
                    Flowrate = item.GetPayloadSingle();
                    break;
                case PumpRegisters.ProtocolStepPeriod:
                    StepPeriod = item.GetPayloadUInt16();
                    break;
                case PumpRegisters.ProtocolVolume:
                    Volume = item.GetPayloadSingle();
                    break;
                case PumpRegisters.ProtocolType:
                    ProtocolType = item.GetPayloadByte();
                    break;
                case PumpRegisters.CalibrationValue1:
                    CalibrationValue1 = item.GetPayloadByte();
                    break;
                case PumpRegisters.CalibrationValue2:
                    CalibrationValue2 = item.GetPayloadByte();
                    break;
                case PumpRegisters.ProtocolDirection:
                    ProtocolDirection = (Direction)item.GetPayloadByte();
                    break;
                case PumpRegisters.EventsEnable:
                    byte all = item.GetPayloadByte();

                    StepStateEvent = GetBit(all, 0);
                    DirectionStateEvent = GetBit(all, 1);
                    SwitchForwardStateEvent = GetBit(all, 2);
                    SwitchReverseStateEvent = GetBit(all, 3);
                    InputStateEvent = GetBit(all, 4);
                    ProtocolStateEvent = GetBit(all, 5);

                    break;
                case PumpRegisters.SetBoardType:
                    break;
                case PumpRegisters.ProtocolState:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool GetBit(byte b, int pos)
        {
            return Convert.ToBoolean((b >> pos) & 1);
        }
    }
}
