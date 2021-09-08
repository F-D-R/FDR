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
        Verify,
        Diff,
        Cleanup
    }

    public class Program
    {
        private const ConsoleColor titleColor = ConsoleColor.White;
        private static Operation operation = Operation.Help;

        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            bool verbose = false;
            bool auto = false;
            bool force = false;
            string folder = string.Empty;
            string file = string.Empty;
            string reference = string.Empty;

            //var services = ConfigureServices();
            //var serviceProvider = services.BuildServiceProvider();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-import": operation = Operation.Import; break;
                    case "-hash": operation = Operation.Hash; folder = args[i + 1]; break;
                    case "-rehash": operation = Operation.Hash; folder = args[i + 1]; force = true; break;
                    case "-verify": operation = Operation.Verify; folder = args[i + 1]; break;
                    case "-diff": operation = Operation.Diff; folder = args[i + 1]; break;
                    case "-cleanup": operation = Operation.Cleanup; folder = args[i + 1]; break;
                    case "-verbose": verbose = true; break;
                    case "-auto": auto = true; break;
                    case "-folder": folder = args[i + 1]; break;
                    case "-file": file = args[i + 1]; break;
                    case "-reference": reference = args[i + 1]; break;
                }
            }

            using (ConsoleTraceListener consoleTracer = new ConsoleTraceListener())
            {
                if (verbose) Trace.Listeners.Add(consoleTracer);
                try
                {
                    switch (operation)
                    {
                        case Operation.Import:
                            Common.Msg($"FDR Tools {version} - Import", titleColor);
                            var appConfig = LoadAppConfig();
                            Import.ImportWizard(appConfig.ImportConfigs, auto);
                            break;

                        case Operation.Hash:
                            Common.Msg($"FDR Tools {version} - Hash", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            Verify.HashFolder(new DirectoryInfo(Path.GetFullPath(folder)), force);
                            break;

                        case Operation.Verify:
                            Common.Msg($"FDR Tools {version} - Verify", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            Verify.VerifyFolder(new DirectoryInfo(Path.GetFullPath(folder)));
                            break;

                        case Operation.Diff:
                            Common.Msg($"FDR Tools {version} - Diff", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            if (!Common.IsFolderValid(reference)) return;
                            Verify.DiffFolder(new DirectoryInfo(Path.GetFullPath(folder)), new DirectoryInfo(Path.GetFullPath(reference)));
                            break;

                        case Operation.Cleanup:
                            Common.Msg($"FDR Tools {version} - Cleanup", titleColor);
                            if (!Common.IsFolderValid(folder)) return;
                            Raw.CleanupFolder(new DirectoryInfo(Path.GetFullPath(folder)));
                            break;

                        default:
                            DisplayHelp(version);
                            break;
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

        private static void DisplayHelp(string version)
        {
            Common.Msg($"FDR Tools {version} - Help", titleColor);
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
            Common.Msg("    -verbose             More detailed output");
            Common.Msg("    -import              Import memory card content");
            Common.Msg("    -hash <folder>       Create hash of files in a folder");
            Common.Msg("    -rehash <folder>     Recreate hashes of all files in a folder");
            Common.Msg("    -verify <folder>     Verify the files in a folder");
            Common.Msg("    -diff <folder>       Compare the files of a folder to a reference one");
            Common.Msg("    -cleanup <folder>    Delete unnecessary raw and hash files");
            Common.Msg("    -reference <folder>  Reference folder");
            Common.Msg("    -auto                Automatic start");
        }

        private static AppConfig LoadAppConfig()
        {
            var appPath = Assembly.GetExecutingAssembly().Location;
            var configPath = Path.Combine(Path.GetDirectoryName(appPath), "appsettings.json");
            return JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configPath, Encoding.UTF8));
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
