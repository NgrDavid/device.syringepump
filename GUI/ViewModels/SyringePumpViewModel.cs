using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.Pump.GUI.ViewModels
{
    public class SyringePumpViewModel : ViewModelBase
    {
        public string AppVersion { get; set; }

        public SyringePumpViewModel()
        {
            AppVersion = "v"+ typeof(SyringePumpViewModel).Assembly.GetName().Version?.ToString(3);
        }
    }
}
