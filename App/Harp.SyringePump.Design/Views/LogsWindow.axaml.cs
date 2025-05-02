using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SyringePump.Design.ViewModels;

namespace SyringePump.Design.Views
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
