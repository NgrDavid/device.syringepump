#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Bonsai;
using Bonsai.Harp;
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

        [Reactive] public ObservableCollection<string> HarpMessages { get; set; }

        [Reactive] public bool StepStateEvent { get; set; } = true;
        [Reactive] public bool DirectionStateEvent { get; set; } = true;
        [Reactive] public bool SwitchForwardStateEvent { get; set; } = true;
        [Reactive] public bool SwitchReverseStateEvent { get; set; } = true;
        [Reactive] public bool InputStateEvent { get; set; } = true;

        [Reactive] public int ProtocolType { get; set; } = 0;
        
        [Reactive] public int NumberOfSteps { get; set; } = 15;
        [Reactive] public int StepPeriod { get; set; } = 10;
        [Reactive] public float Flowrate { get; set; } = 0.5f;
        [Reactive] public float Volume { get; set; } = 0.5f;
        [Reactive] public int MotorMicrostep { get; set; } = 0;
        [Reactive] public int DigitalInput0Config { get; set; } = 0;
        [Reactive] public int DigitalOutput0Config { get; set; } = 0;
        [Reactive] public int DigitalOutput1Config { get; set; } = 0;

        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<string, Unit> ConnectAndGetBaseInfoCommand{ get; }
        public ReactiveCommand<bool, Unit> SaveConfigurationCommand{ get; }
        private Bonsai.Harp.Device _dev;

        public SyringePumpViewModel()
        {
            AppVersion = "v"+ typeof(SyringePumpViewModel).Assembly.GetName().Version?.ToString(3);

            LoadDeviceInformation = ReactiveCommand.Create(LoadUSBInformation);

            var canConnect = this.WhenAnyValue(x => x.SelectedPort)
                .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

            ConnectAndGetBaseInfoCommand = ReactiveCommand.Create<string>(ConnectAndGetBaseInfo, canConnect);
            ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine(ex.Message));

            SaveConfigurationCommand = ReactiveCommand.Create<bool>(SaveConfiguration);
            HarpMessages = new ObservableCollection<string>();
            
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

        public void LoadUSBInformation()
        {
            var devices = SerialPort.GetPortNames();

            if (OperatingSystem.IsMacOS())
                Ports = devices?.Where(d => d.Contains("cu.")).ToList();
            else
                Ports = devices?.ToList();
        }

        private void SaveConfiguration(bool savePermanently)
        {
            if (_dev == null)
                throw new Exception("You need to connect to the device first");
            
            var msgs = new List<HarpMessage>();
            
            if (savePermanently)
            {
                var resetMessage = HarpCommand.Reset(ResetMode.Save);
                msgs.Add(resetMessage);
            }
            else
            {
                // send commands to save every config (verify each step for board's reply)

                // events
                byte events = (byte) ((Convert.ToByte(StepStateEvent) << 0) |
                                      (Convert.ToByte(DirectionStateEvent) << 1) |
                                      (Convert.ToByte(SwitchForwardStateEvent) << 2) |
                                      (Convert.ToByte(SwitchReverseStateEvent) << 3) |
                                      (Convert.ToByte(InputStateEvent) << 4));
                var eventsMessage = HarpCommand.WriteByte(52, events);
                msgs.Add(eventsMessage);

                // motor microstep
                byte motor = Convert.ToByte(MotorMicrostep);
                var motorMessage = HarpCommand.WriteByte(44, motor);
                msgs.Add(motorMessage);

                // di0, do0 and do1
                byte di0 = Convert.ToByte(DigitalInput0Config);
                var di0Message = HarpCommand.WriteByte(43, di0);
                msgs.Add(di0Message);

                byte do0 = Convert.ToByte(DigitalOutput0Config);
                var do0Message = HarpCommand.WriteByte(41, do0);
                msgs.Add(do0Message);

                byte do1 = Convert.ToByte(DigitalOutput1Config);
                var do1Message = HarpCommand.WriteByte(42, do1);
                msgs.Add(do1Message);

                // protocol
                // protocol type
                byte protocolType = Convert.ToByte(ProtocolType);
                var protocolTypeMessage = HarpCommand.WriteByte(49, protocolType);
                msgs.Add(protocolTypeMessage);
                // if step:
                if (protocolType == 0)
                {
                    // number of steps
                    ushort numberOfSteps = Convert.ToUInt16(NumberOfSteps);
                    var numberOfStepsMessage = HarpCommand.WriteUInt16(45, numberOfSteps);
                    msgs.Add(numberOfStepsMessage);

                    // step period
                    ushort stepPeriod = Convert.ToUInt16(StepPeriod);
                    var stepPeriodMessage = HarpCommand.WriteUInt16(47, stepPeriod);
                    msgs.Add(stepPeriodMessage);
                }
                else
                {
                    // flowrate
                    float flowrate = Convert.ToSingle(Flowrate);
                    var flowrateMessage = HarpCommand.WriteSingle(46, flowrate);
                    msgs.Add(flowrateMessage);
                    // volume
                    float volume = Convert.ToSingle(Volume);
                    var volumeMessage = HarpCommand.WriteSingle(48, volume);
                    msgs.Add(volumeMessage);

                    // calibration val 1
                    // calibration val 2
                }
            }
            
            // send all messages independently of the save type
            HarpMessages.Clear();

            var observer = Observer.Create<HarpMessage>(item => HarpMessages.Add(item.ToString()),
                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); },
                () => HarpMessages.Add("Completed sending commands to device"));

            var msgsSubject = new Subject<HarpMessage>();

            var observable = _dev.Generate(msgsSubject)
                .Subscribe(observer);

            foreach (var harpMessage in msgs)
            {
                msgsSubject.OnNext(harpMessage);
            }

            Thread.Sleep(500);
            observable.Dispose();
        }
        private async void ConnectAndGetBaseInfo(string selectedPort)
        {
            if(string.IsNullOrEmpty(selectedPort))
                throw new Exception("invalid parameter");

            _dev = new Bonsai.Harp.Device();
            _dev.PortName = SelectedPort;
            
            // to guarantee that we give enough time to get the data from the device
            Thread.Sleep(200);

            HarpMessages.Clear();
            HarpMessages.Add(((INamedElement)_dev).Name.TrimEnd('\0'));
        }
    }
}
