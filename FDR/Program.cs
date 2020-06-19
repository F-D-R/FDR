using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using FDR.Tools.Library;
using System.Threading;

namespace FDR
{
    public enum Operation
    {
        Help,
        Import,
        Hash,
        Verify
    }

    public class Program
    {
        private static Operation operation = Operation.Help;

        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            bool verbose = false;
            bool auto = false;
            string folder = string.Empty;
            string file = string.Empty;

            //var services = ConfigureServices();
            //var serviceProvider = services.BuildServiceProvider();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-verbose":
                        verbose = true;
                        break;

                    case "-hash":
                        operation = Operation.Hash;
                        break;

                    case "-verify":
                        operation = Operation.Verify;
                        break;

                    case "-import":
                        operation = Operation.Import;
                        break;

                    case "-auto":
                        auto = true;
                        break;

                    case "-folder":
                        folder = args[i + 1];
                        break;

                    case "-file":
                        file = args[i + 1];
                        break;
                }
            }

            using (ConsoleTraceListener consoleTracer = new ConsoleTraceListener())
            {
                if (verbose) Trace.Listeners.Add(consoleTracer);
                try
                {
                    if (operation == Operation.Help)
                    {
                        Common.Msg($"FDR Tools {version} - Help", ConsoleColor.Yellow);
                        Common.Msg("Usage:");
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Common.Msg("    fdr.exe [options]");
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            Common.Msg("    dotnet FDR.dll [options]");
                        else
                        {
                            Common.Msg("    dotnet FDR.dll [options]");
                            Common.Msg("    Unsupported OS version!", ConsoleColor.Red);
                        }
                        Common.Msg("");
                        Common.Msg("Where options can be:");
                        Common.Msg("    -help                Help (this screen)");
                        Common.Msg("    -verbose             Detailed output");
                        Common.Msg("    -import              Import memory card content");
                        Common.Msg("    -hash                Create hash of the files in a folder");
                        Common.Msg("    -verify              Verify the files in a folder");
                        Common.Msg("    -auto                Automatic start");
                        Common.Msg("    -folder <folder>     Subject folder");
                        //Core.Msg("    -file <file>         Subject file");
                    }
                    else if (operation == Operation.Import)
                    {
                        Common.Msg($"FDR Tools {version} - Import", ConsoleColor.Yellow);

                        var appPath = Assembly.GetExecutingAssembly().Location;
                        var configPath = Path.Combine(Path.GetDirectoryName(appPath), "appsettings.json");
                        var appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configPath, Encoding.UTF8));

                        Import.ImportWizard(appConfig.ImportConfigs, auto);
                    }
                    else if (operation == Operation.Hash)
                    {
                        Common.Msg($"FDR Tools {version} - Hash", ConsoleColor.Yellow);

                        if (!Common.IsFolderValid(folder)) return;
                        folder = Path.GetFullPath(folder);

                        Verify.HashFolder(folder);
                    }
                    else if (operation == Operation.Verify)
                    {
                        Common.Msg($"FDR Tools {version} - Verify", ConsoleColor.Yellow);

                        if (!Common.IsFolderValid(folder)) return;
                        folder = Path.GetFullPath(folder);

                        Verify.VerifyFolder(folder);
                    }
                }
                finally
                {
                    Trace.Flush();
                    Trace.Listeners.Remove(consoleTracer);
                    consoleTracer.Close();
                }
            }
        }

        //private static IServiceCollection ConfigureServices()
        //{
        //    IServiceCollection services = new ServiceCollection();

        //    //var config = LoadConfiguration();
        //    //services.AddSingleton(config);

        //    // required to run the application
        //    //services.AddTransient<App>();

        //    var myLogger = new App();
        //    services.AddSingleton(myLogger);

        //    return services;
        //}
    }


    //public interface IApp
    //{
    //    void Msg(string msg, ConsoleColor color = ConsoleColor.White);
    //}

    //public class App : IApp
    //{
    //    //private readonly ILogger<MyLogger> _logger;

    //    //public MyLogger(ILogger<MyLogger> logger)
    //    //{
    //    //    _logger = logger;
    //    //}

    //    public void Msg(string msg, ConsoleColor color = ConsoleColor.White)
    //    {
    //        Console.BackgroundColor = ConsoleColor.Black;
    //        Console.ForegroundColor = color;
    //        Console.WriteLine(msg);
    //        Console.ResetColor();
    //    }
    //}
}
