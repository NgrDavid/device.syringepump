using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Device.Pump.GUI.ViewModels;
using Device.Pump.GUI.Views;

namespace Device.Pump
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void NativeMenuItem_OnClick(object sender, EventArgs e)
        { 
            // FIXME: This should be using a Command
            var about = new About() { DataContext = new AboutViewModel() };
            about.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
                .MainWindow);
        }
    }
}
