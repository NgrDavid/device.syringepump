// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Serilog;
using Serilog.Events;

namespace SyringePump.Design
{
    public class StartApp
    {
        public static AppBuilder BuildAvaloniaApp()
        {
            var log = new LoggerConfiguration()
                .WriteTo.File($"logs{Path.DirectorySeparatorChar}log.txt",
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
        
        public static void CloseLog()
        {
            // properly close the log on app close
            Log.CloseAndFlush();
        }
    }
}
