#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using Bonsai;
using Bonsai.Harp;
using Device.Pump.GUI.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

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

        [Reactive] public int ProtocolType { get; set; }

        [Reactive] public string DeviceName { get; set; }
        [Reactive] public int DeviceID { get; set; }
        [Reactive] public HarpVersion HardwareVersion { get; set; }
        [Reactive] public HarpVersion FirmwareVersion { get; set; }
        
        [Reactive] public int NumberOfSteps { get; set; } = 15;
        [Reactive] public int StepPeriod { get; set; } = 10;
        [Reactive] public float Flowrate { get; set; } = 0.5f;
        [Reactive] public float Volume { get; set; } = 0.5f;
        [Reactive] public int MotorMicrostep { get; set; } = 0;
        [Reactive] public int DigitalInput0Config { get; set; } = 0;
        [Reactive] public int DigitalOutput0Config { get; set; } = 0;
        [Reactive] public int DigitalOutput1Config { get; set; }
        [Reactive] public int CalibrationValue1 { get; set; }
        [Reactive] public int CalibrationValue2 { get; set; }

        [ObservableAsProperty]
        public bool IsLoadingPorts { get; }

        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<string, Unit> ConnectAndGetBaseInfoCommand{ get; }
        public ReactiveCommand<bool, Unit> SaveConfigurationCommand{ get; }
        public ReactiveCommand<Unit, Unit> ResetConfigurationCommand{ get; }

        private Bonsai.Harp.Device _dev;
        private Subject<HarpMessage> _msgsSubject;

        public SyringePumpViewModel()
        {
            AppVersion = "v"+ typeof(SyringePumpViewModel).Assembly.GetName().Version?.ToString(3);

            LoadDeviceInformation = ReactiveCommand.CreateFromObservable(LoadUSBInformation);
            LoadDeviceInformation.IsExecuting.ToPropertyEx(this, x => x.IsLoadingPorts);

            var canConnect = this.WhenAnyValue(x => x.SelectedPort)
                .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

            ConnectAndGetBaseInfoCommand = ReactiveCommand.Create<string>(ConnectAndGetBaseInfo, canConnect);
            ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine(ex.Message));

            var canChangeConfig = this.WhenAnyValue(x => x.Connected).Select(connected => connected);
            SaveConfigurationCommand = ReactiveCommand.Create<bool>(SaveConfiguration, canChangeConfig);
            ResetConfigurationCommand = ReactiveCommand.Create(ResetConfiguration, canChangeConfig);

            HarpMessages = new ObservableCollection<string>();
            // TODO: missing properly dispose of this
            _msgsSubject = new Subject<HarpMessage>();
            
            // Validation rules
            this.ValidationRule(viewModel => viewModel.NumberOfSteps,
                steps => steps > 0 && steps < 65535,
                "Number of steps only accepts integer values between 1 and 65535");
            this.ValidationRule(viewModel => viewModel.StepPeriod,
                period => period > 0 && period < 65535,
                "Step Period only accepts integer values between 1 and 65535");
            this.ValidationRule(viewModel => viewModel.Flowrate,
                flowRate => flowRate >= 0.5f && flowRate <= 2000f,
                "Flowrate only accepts float values between 0.5 and 2000");
            this.ValidationRule(viewModel => viewModel.Volume,
                volume => volume >= 0.5f && volume <= 2000f,
                "Volume only accepts float values between 0.5 and 2000");

            // force initial population of currently connected ports
            LoadUSBInformation();
        }

        public IObservable<Unit> LoadUSBInformation()
        {
            return Observable.Start(() =>
            {
                var devices = SerialPort.GetPortNames();

                if (OperatingSystem.IsMacOS())
                    Ports = devices?.Where(d => d.Contains("cu.")).ToList();
                else
                    Ports = devices?.ToList();
            });
        }

        private void SaveConfiguration(bool savePermanently)
        {
            if (_dev == null)
                throw new Exception("You need to connect to the device first");

            var msgs = new List<HarpMessage>();

            // send commands to save every config (verify each step for board's reply)

            // events
            byte events = (byte) ((Convert.ToByte(StepStateEvent) << 0) |
                                  (Convert.ToByte(DirectionStateEvent) << 1) |
                                  (Convert.ToByte(SwitchForwardStateEvent) << 2) |
                                  (Convert.ToByte(SwitchReverseStateEvent) << 3) |
                                  (Convert.ToByte(InputStateEvent) << 4));
            var eventsMessage = HarpCommand.WriteByte((int) PumpRegisters.EventsEnable, events);
            msgs.Add(eventsMessage);

            // motor microstep
            byte motor = Convert.ToByte(MotorMicrostep);
            var motorMessage = HarpCommand.WriteByte((int) PumpRegisters.MotorMicrostep, motor);
            msgs.Add(motorMessage);

            // di0, do0 and do1
            byte di0 = Convert.ToByte(DigitalInput0Config);
            var di0Message = HarpCommand.WriteByte((int) PumpRegisters.DigitalInput0Config, di0);
            msgs.Add(di0Message);

            byte do0 = Convert.ToByte(DigitalOutput0Config);
            var do0Message = HarpCommand.WriteByte((int) PumpRegisters.DigitalOutput0Config, do0);
            msgs.Add(do0Message);

            byte do1 = Convert.ToByte(DigitalOutput1Config);
            var do1Message = HarpCommand.WriteByte((int) PumpRegisters.DigitalOutput1Config, do1);
            msgs.Add(do1Message);

            // protocol
            // protocol type
            byte protocolType = Convert.ToByte(ProtocolType);
            var protocolTypeMessage = HarpCommand.WriteByte((int) PumpRegisters.ProtocolType, protocolType);
            msgs.Add(protocolTypeMessage);
            // if step:
            if (protocolType == 0)
            {
                // number of steps
                ushort numberOfSteps = Convert.ToUInt16(NumberOfSteps);
                var numberOfStepsMessage =
                    HarpCommand.WriteUInt16((int) PumpRegisters.ProtocolNumberOfSteps, numberOfSteps);
                msgs.Add(numberOfStepsMessage);

                // step period
                ushort stepPeriod = Convert.ToUInt16(StepPeriod);
                var stepPeriodMessage = HarpCommand.WriteUInt16((int) PumpRegisters.ProtocolStepPeriod, stepPeriod);
                msgs.Add(stepPeriodMessage);
            }
            else
            {
                // flowrate
                float flowrate = Convert.ToSingle(Flowrate);
                var flowrateMessage = HarpCommand.WriteSingle((int) PumpRegisters.ProtocolFlowrate, flowrate);
                msgs.Add(flowrateMessage);
                // volume
                float volume = Convert.ToSingle(Volume);
                var volumeMessage = HarpCommand.WriteSingle((int) PumpRegisters.ProtocolVolume, volume);
                msgs.Add(volumeMessage);

                // calibration val 1
                byte calValue1 = Convert.ToByte(CalibrationValue1);
                var calValue1Message = HarpCommand.WriteByte((int) PumpRegisters.CalibrationValue1, calValue1);
                msgs.Add(calValue1Message);

                // calibration val 2
                byte calValue2 = Convert.ToByte(CalibrationValue2);
                var calValue2Message = HarpCommand.WriteByte((int) PumpRegisters.CalibrationValue2, calValue2);
                msgs.Add(calValue2Message);
            }
            

            if (savePermanently)
            {
                var resetMessage = HarpCommand.Reset(ResetMode.Save);
                msgs.Add(resetMessage);
            }

            // send all messages independently of the save type
            HarpMessages.Clear();

            var observer = Observer.Create<HarpMessage>(item => HarpMessages.Add(item.ToString()),
                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); },
                () => HarpMessages.Add("Completed sending commands to device"));

            // TODO: missing properly dispose of this observable
            var observable = _dev.Generate(_msgsSubject)
                .Subscribe(observer);

            foreach (var harpMessage in msgs)
            {
                _msgsSubject.OnNext(harpMessage);
            }

            Thread.Sleep(500);
            observable.Dispose();
        }

        private void ResetConfiguration()
        {
            var resetMessage = HarpCommand.Reset(ResetMode.RestoreDefault);

            var observer = Observer.Create<HarpMessage>(item => HarpMessages.Add(item.ToString()),
                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); },
                () => HarpMessages.Add("Completed sending commands to device"));
            
            var observable = _dev.Generate(_msgsSubject)
                .Subscribe(observer);

            _msgsSubject.OnNext(resetMessage);

            Thread.Sleep(200);

            // send message to opened device
            // //TODO: when we have the observable from the receiving data, we should present a message stating if the operation completed successfully
        }

        private async void ConnectAndGetBaseInfo(string selectedPort)
        {
            if(string.IsNullOrEmpty(selectedPort))
                throw new Exception("invalid parameter");

            StringBuilder sb = new StringBuilder();
            var writer = new StringWriter(sb);
            Console.SetOut(writer);

            _dev = new Bonsai.Harp.Device();
            _dev.PortName = SelectedPort;
            
            // to guarantee that we give enough time to get the data from the device
            Thread.Sleep(150);

            HarpMessages.Clear();

            var observer = Observer.Create<HarpMessage>(UpdateUI,
                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); },
                () => HarpMessages.Add("Completed sending commands to device"));
            
            var observable = _dev.Generate().Where(item => item.MessageType == MessageType.Read && item.Address >= (int)(PumpRegisters.EnableMotorDriver))
                .Subscribe(observer);

            Thread.Sleep(300);

            Connected = true;

            var info = sb.ToString().Split(Environment.NewLine);
            // [1] = WhoAmI: 1280
            // [2] = Hw: 1.0
            // [3] = Fw: 1.0
            // [5] = DeviceName: Pump \0\0\0\0\0\0
            if (info.Length < 6)
            {
                // TODO: something went wrong, handle this
            }

            int id = Convert.ToInt32(info[1].Split(':')[1].Trim());

            DeviceName = ((INamedElement) _dev).Name.TrimEnd('\0').ToUpper();
            DeviceID = id;

            // convert Hw and Fw version
            HardwareVersion = HarpVersion.Parse(info[2].Split(':')[1].Trim());
            FirmwareVersion = HarpVersion.Parse(info[3].Split(':')[1].Trim());

            writer.Close();

            // return Console output to default
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            await writer.DisposeAsync();
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
                case PumpRegisters.EventsEnable:
                    byte all = item.GetPayloadByte();

                    StepStateEvent = GetBit(all, 0);
                    DirectionStateEvent = GetBit(all, 1);
                    SwitchForwardStateEvent = GetBit(all, 2);
                    SwitchReverseStateEvent = GetBit(all, 3);
                    InputStateEvent = GetBit(all, 4);

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
