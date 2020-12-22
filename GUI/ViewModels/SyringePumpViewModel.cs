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
        private Bonsai.Harp.Device _dev;

        public SyringePumpViewModel()
        {
            AppVersion = "v"+ typeof(SyringePumpViewModel).Assembly.GetName().Version?.ToString(3);

            LoadDeviceInformation = ReactiveCommand.Create(LoadUSBInformation);

            var canConnect = this.WhenAnyValue(x => x.SelectedPort)
                .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

            ConnectAndGetBaseInfoCommand = ReactiveCommand.Create<string>(ConnectAndGetBaseInfo, canConnect);
            ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine(ex.Message));

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
