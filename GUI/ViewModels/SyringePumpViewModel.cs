#region Usings

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using Bonsai.Harp;
using Device.Pump.GUI.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Device.Pump.GUI.ViewModels
{
    public class SyringePumpViewModel : ViewModelBase
    {
        public string AppVersion { get; set; }

        [Reactive] public List<string> Ports { get; set; }

        [Reactive] public string SelectedPort { get; set; }

        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<string, Unit> ConnectAndGetBaseInfoCommand{ get; }

        public SyringePumpViewModel()
        {
            AppVersion = "v"+ typeof(SyringePumpViewModel).Assembly.GetName().Version?.ToString(3);

            LoadDeviceInformation = ReactiveCommand.Create(LoadUSBInformation);

            var canConnect = this.WhenAnyValue(x => x.SelectedPort)
                .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

            ConnectAndGetBaseInfoCommand = ReactiveCommand.Create<string>(ConnectAndGetBaseInfo, canConnect);
            ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine(ex.Message));

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
        }
    }
}
