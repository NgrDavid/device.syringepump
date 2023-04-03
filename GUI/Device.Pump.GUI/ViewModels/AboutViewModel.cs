using Avalonia.Themes.Fluent;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Device.Pump.GUI.ViewModels
{
    public class AboutViewModel : ReactiveObject
    {
        [Reactive] public bool ShowDarkTheme { get; set; }

        public AboutViewModel()
        {
            // Get current theme
            ShowDarkTheme = ((FluentTheme)App.Current.Styles[0]).Mode == FluentThemeMode.Dark;
        }
    }
}
