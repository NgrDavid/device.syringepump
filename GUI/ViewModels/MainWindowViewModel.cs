using System;
using System.Collections.Generic;
using System.Text;

namespace Device.Pump.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public SyringePumpViewModel SyringePump { get; set; }

        public MainWindowViewModel()
        {
            SyringePump = new SyringePumpViewModel();
        }
    }
}
