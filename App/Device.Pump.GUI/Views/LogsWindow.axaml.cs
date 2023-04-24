using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Device.Pump.GUI.ViewModels;

namespace Device.Pump.GUI.Views
{
    public partial class LogsWindow : ReactiveUserControl<LogsWindowViewModel>
    {
        public LogsWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
