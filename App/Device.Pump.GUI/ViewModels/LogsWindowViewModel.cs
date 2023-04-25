using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Device.Pump.GUI.ViewModels
{
    public class LogsWindowViewModel : ReactiveObject
    {
        [Reactive] public ObservableCollection<string> HarpMessages { get; set; }
    }
}
