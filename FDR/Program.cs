using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        Verify
    }

    public class Program
    {
        private static Operation operation;

        public static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            bool verbose = false;
            bool auto = false;
            bool recursive = false;
            string folder = string.Empty;
            string file = string.Empty;

            //var services = ConfigureServices();
            //var serviceProvider = services.BuildServiceProvider();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/?":
                    case "-?":
                    case "-h":
                    case "--help":
                        operation = Operation.Help;
                        break;

                    case "-v":
                    case "--verbose":
                        verbose = true;
                        break;

                    case "--verify":
                        operation = Operation.Verify;
                        break;

                    case "-i":
                    case "--import":
                        operation = Operation.Import;
                        break;

                    case "-a":
                    case "--auto":
                        auto = true;
                        break;

                    case "--folder":
                        folder = args[i + 1];
                        break;

                    case "--file":
                        file = args[i + 1];
                        break;

                    case "-r":
                    case "--recursive":
                        recursive = true;
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
                        Msg($"FDR Tools {version} - Help", ConsoleColor.Yellow);
                        Msg("Usage:");
                        Msg("    fdr.exe [options]");
                        Msg("");
                        Msg("Where options can be:");
                        Msg("    -h    --help         Help (this screen)");
                        Msg("    -v    --verbose      Detailed output");
                        Msg("    -i    --import       Import memory card content");
                        Msg("          --verify       Verify the files in a folder");
                        Msg("    -a    --auto         Automatic start");
                        Msg("    -r    --recursive    Recursive folder operation");
                        Msg("    --folder <folder>    Subject folder");
                        //Msg("    --file <file>        Subject file");
                    }
                    else if (operation == Operation.Import)
                    {
                        Msg($"FDR Tools {version} - Import", ConsoleColor.Yellow);

                        var appPath = Assembly.GetExecutingAssembly().Location;
                        var configPath = Path.Combine(Path.GetDirectoryName(appPath), "appsettings.json");
                        var appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configPath, Encoding.UTF8));

                        Import.ImportWizard(appConfig.ImportConfigs, auto);
                    }
                    else if (operation == Operation.Verify)
                    {
                        Msg($"FDR Tools {version} - Verify", ConsoleColor.Yellow);

                        //Trace.WriteLine($"Folder: {folder}");
                        //Trace.WriteLine($"File: {file}");

                        if (string.IsNullOrWhiteSpace(folder))
                        {
                            Msg("Folder name is missing!", ConsoleColor.Red);
                            return;
                        }
                        if (!Directory.Exists(folder))
                        {
                            Msg("Folder must be an existing one!", ConsoleColor.Red);
                            return;
                        }
                        folder = Path.GetFullPath(folder);
                        Msg($"Verifying folder {folder}");

                        Verify.VerifyFolder(folder, recursive);
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

        private static void Msg(string msg, ConsoleColor color = ConsoleColor.White, bool newline = true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            if (newline)
                Console.WriteLine(msg);
            else
                Console.Write(msg);
            Console.ResetColor();
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
