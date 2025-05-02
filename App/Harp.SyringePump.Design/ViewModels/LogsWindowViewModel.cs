using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SyringePump.Design.ViewModels
{
    public class LogsWindowViewModel : ReactiveObject
    {
        [Reactive] public ObservableCollection<string> HarpMessages { get; set; }
    }
}
