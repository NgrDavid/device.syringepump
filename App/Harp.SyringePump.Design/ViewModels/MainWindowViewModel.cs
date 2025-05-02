using ReactiveUI;

namespace SyringePump.Design.ViewModels
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
