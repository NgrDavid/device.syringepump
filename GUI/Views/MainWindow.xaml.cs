using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Device.Pump.GUI.ViewModels;

namespace Device.Pump.GUI.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}