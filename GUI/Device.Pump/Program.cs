using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using Serilog;
using Serilog.Events;
using Device.Pump.GUI;
using ReactiveUI;

namespace Device.Pump
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

            // properly close the log on app close
            Log.CloseAndFlush();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            var log = new LoggerConfiguration()
                        .WriteTo.File("log.txt",
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true)
                        .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
#if !DEBUG
                        .WriteTo.Sentry( o =>
                        {
                            o.InitializeSdk = true;
                            o.MinimumBreadcrumbLevel = LogEventLevel.Debug; // Debug and higher are stored as breadcrumbs (default os Information)
                            o.MinimumEventLevel = LogEventLevel.Error; // Error and higher is sent as event (default is Error)
                            o.Dsn = "https://b215ec2ccf9e4ff894185b063c3c7e57@o319014.ingest.sentry.io/5552288";
                            o.AttachStacktrace = true;
                        })
#endif
                        .CreateLogger();

            Log.Logger = log;

            RxApp.DefaultExceptionHandler = new MyCustomObservableExceptionHandler();

            Log.Information("Starting application");

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}
