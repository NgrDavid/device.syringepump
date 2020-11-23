using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Text;
using Bonsai.Harp;
using ReactiveUI;

namespace Device.Pump.GUI.ViewModels
{
    public class SyringePumpViewModel : ViewModelBase
    {
        public string AppVersion { get; set; }

        private List<string> _ports = new List<string>();

        public List<string> Ports
        {
            get => _ports;
            set => this.RaiseAndSetIfChanged(ref _ports, value);
        }

        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }

        public SyringePumpViewModel()
        {
            AppVersion = "v"+ typeof(SyringePumpViewModel).Assembly.GetName().Version?.ToString(3);

            LoadDeviceInformation = ReactiveCommand.Create(LoadUSBInformation);

            // force initial population of currently connected ports
            LoadUSBInformation();
        }

        public void LoadUSBInformation()
        {
            var devices = SerialPort.GetPortNames();
            Ports = devices?.ToList();
        }
    }
}
