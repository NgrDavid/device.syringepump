using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SyringePump.Design.ViewModels;

namespace SyringePump.Design.Views
{
    public partial class About : ReactiveWindow<AboutViewModel>
    {
        public About()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

