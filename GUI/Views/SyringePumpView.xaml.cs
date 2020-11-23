using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Device.Pump.GUI.Views
{
    public class SyringePumpView : UserControl
    {
        public SyringePumpView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}