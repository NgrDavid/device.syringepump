using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SyringePump.Design.ViewModels;

namespace SyringePump.Design.Views
{
    public class SyringePumpView : ReactiveUserControl<SyringePumpViewModel>
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
