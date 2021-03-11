using ReactiveUI;

namespace Device.Pump.GUI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public SyringePumpViewModel SyringePump { get; set; }

        public MainWindowViewModel()
        {
            SyringePump = new SyringePumpViewModel();
        }
    }
}
