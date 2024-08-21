using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using FDR.Tools.Library;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace FDR
{
    public class Program
    {
        private const ConsoleColor titleColor = ConsoleColor.White;
        private const ConsoleColor errorColor = ConsoleColor.Red;
        private static Operation operation = Operation.Help;
        private static readonly string version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        private static bool verbose = false;
        private static bool auto = false;
        private static bool force = false;
        private static bool noactions = false;
        private static string folder = "";
        private static string file = "";
        private static string configfile = "";
        private static string reference = "";
        private static string config = "";
        private static string function = "";
        private static readonly CancellationTokenSource tokenSource = new();
        private static CancellationToken token; // = new();
        private static string? URL = null;


        public static void Main(string[] args)
        {
            //AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            //AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;


            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            URL = configuration["Kestrel:Endpoints:Https:Url"];


            token = tokenSource.Token;

            Console.CancelKeyPress += (s, e) =>
            {
                //Common.Msg("CancelKeyPress...", ConsoleColor.Red);
                tokenSource.Cancel();
                Thread.Sleep(500);
            };


            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case Common.param_import:
                        operation = Operation.Import;
                        GetParam(ref folder, nameof(folder), true);
                        break;

                    case Common.param_hash:
                        operation = Operation.Hash;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_rehash:
                        operation = Operation.Hash;
                        force = true;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_verify:
                        operation = Operation.Verify;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_diff:
                        operation = Operation.Diff;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_cleanup:
                        operation = Operation.Cleanup;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_folderinfo:
                        operation = Operation.FolderInfo;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_verbose:
                        verbose = true;
                        break;

                    case Common.param_auto:
                        auto = true;
                        break;

                    case Common.param_noactions:
                        noactions = true;
                        break;

                    case Common.param_force:
                        force = true;
                        break;

                    case Common.param_file:
                        GetParam(ref file, nameof(file));
                        break;

                    case Common.param_reference:
                        GetParam(ref reference, nameof(reference));
                        break;

                    case Common.param_rename:
                        operation = Operation.Rename;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_resize:
                        operation = Operation.Resize;
                        GetParam(ref folder, nameof(folder));
                        break;

                    case Common.param_web:
                    case "-ui":
                        operation = Operation.Web;
                        break;

                    case Common.param_config:
                        GetParam(ref config, nameof(config));
                        break;

                    case Common.param_configfile:
                        GetParam(ref configfile, nameof(configfile));
                        break;

                    case Common.param_help:
                        operation = Operation.Help;
                        GetParam(ref function, nameof(function), true);
                        break;
                }

                void GetParam(ref string param, string paramname, bool optional = false)
                {
                    if (args.Length <= i + 1 || args[i + 1].StartsWith("-"))
                    {
                        if (!optional)
                        {
                            Common.Msg($"FDR Tools {version} - ERROR", titleColor);
                            Common.Msg($"Missing parameter: {paramname}!", errorColor);
                            OfferHelpAndExit();
                        }
                    }
                    else
                    {
                        param = args[i + 1];
                        i++;
                    }
                }
            }

            using ConsoleTraceListener consoleTracer = new();
            AppConfig appConfig;
            if (verbose) Trace.Listeners.Add(consoleTracer);
            try
            {
                switch (operation)
                {
                    case Operation.Import:
                        Common.Msg($"FDR Tools {version} - Import", titleColor);
                        appConfig = AppConfig.Load(configfile);
                        if (appConfig.ImportConfigs == null)
                        {
                            Common.Msg("There are no import configurations!", errorColor);
                            return;
                        }
                        Import.ImportWizard(appConfig.ImportConfigs, string.IsNullOrWhiteSpace(folder) ? null : new DirectoryInfo(Path.GetFullPath(folder)), auto, noactions, force);
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
                        Raw.CleanupFolder(new DirectoryInfo(Path.GetFullPath(folder)), token);
                        break;

                    case Operation.Rename:
                        Common.Msg($"FDR Tools {version} - Rename", titleColor);
                        if (!Common.IsFolderValid(folder)) return;
                        if (string.IsNullOrWhiteSpace(config))
                        {
                            Common.Msg("Rename configuration is not defined!", errorColor);
                            return;
                        }
                        appConfig = AppConfig.Load(configfile);
                        RenameConfig? renameConfig;
                        if (!appConfig.RenameConfigs.TryGetValue(config, out renameConfig))
                        {
                            Common.Msg("Given rename configuration does not exist!", errorColor);
                            return;
                        }
                        Rename.RenameFilesInFolder(new DirectoryInfo(Path.GetFullPath(folder)), renameConfig);
                        break;

                    case Operation.Resize:
                        Common.Msg($"FDR Tools {version} - Resize", titleColor);
                        if (!Common.IsFolderValid(folder)) return;
                        if (string.IsNullOrWhiteSpace(config))
                        {
                            Common.Msg("Resize configuration is not defined!", errorColor);
                            return;
                        }
                        appConfig = AppConfig.Load(configfile);
                        ResizeConfig? resizeConfig;
                        if (!appConfig.ResizeConfigs.TryGetValue(config, out resizeConfig))
                        {
                            Common.Msg("Given resize configuration does not exist!", errorColor);
                            return;
                        }
                        Resize.ResizeFilesInFolder(new DirectoryInfo(Path.GetFullPath(folder)), resizeConfig);
                        break;

                    case Operation.Web:
                        Common.Msg($"FDR Tools {version} - Web", titleColor);
                        if (string.IsNullOrEmpty(URL))
                        {
                            Common.Msg("Missing URL configuration!", errorColor);
                            return;
                        }
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Process.Start("FDR.Web.exe");
                        else
                            Process.Start("dotnet", "FDR.Web.dll");
                        Process.Start("explorer", URL);
                        break;

                    case Operation.FolderInfo:
                        Common.Msg($"FDR Tools {version} - FolderInfo", titleColor);
                        if (!Common.IsFolderValid(folder)) return;
                        Raw.PrintFolderInfo(new DirectoryInfo(Path.GetFullPath(folder)), token);
                        break;

                    default:
                        DisplayHelp(version, function);
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

        private static void OnProcessExit(object? sender, EventArgs e)
        {
            Common.Msg($"OnProcessExit...", titleColor);
        }

        //private static void OnDomainUnload(object? sender, EventArgs e)
        //{
        //    Common.Msg($"OnDomainUnload...", titleColor);
        //}

        private static void OfferHelpAndExit()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Common.Msg("See help for details: FDR.exe -help");
            else
                Common.Msg("See help for details: dotnet FDR.dll -help");
            Environment.Exit(-1);
        }

        private static void DisplayHelp(string version, string? function = null)
        {
            var func = function?.Trim().ToLower();
            Common.Msg($"FDR Tools {version} - Help" + (string.IsNullOrWhiteSpace(func) ? "" : ": " + func), titleColor);
            Common.Msg(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrWhiteSpace(func))
            {
                Common.Msg("Usage:");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Common.Msg("  FDR.exe [options]");
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    Common.Msg("  dotnet FDR.dll [options]");
                else
                {
                    Common.Msg("  dotnet FDR.dll [options]");
                    Common.Msg("  Unsupported OS version!", errorColor);
                }
                Common.Msg("");
                Common.Msg("Where options can be:");
                Common.ShowAttributeHelp(Common.GetProgramParameterList(), false);
            }
            else
            {
                if (func =="import")
                    Import.ShowImportHelp();
                else if (func == "hash")
                    Common.Msg("\nCreates hidden hash files of files in the folder given after the -hash option for which there were none yet.");
                else if (func == "rehash")
                    Common.Msg("\nRecreates the hidden hash files of all files in the folder given after the -rehash option whether they existed or not.");
                else if (func == "verify")
                    Common.Msg("\nVerifies the files in the folder given after the -verify option against their saved hidden hash and creates error files whenever there are differences.");
                else if (func == "diff")
                    Common.Msg("\nCompares the files of the folder given after the -diff option to a reference folder given after the -reference option.");
                else if (func == "cleanup")
                    Common.Msg("\nDeletes unnecessary raw, hash and err files.");
                else if (func == "rename")
                    Rename.ShowRenameHelp();
                else if (func == "move")
                    Import.ShowMoveHelp();
                else if (func == "resize")
                    Resize.ShowResizeHelp();
                else
                {
                    Common.Msg("Invalid function: " + func, errorColor);
                    OfferHelpAndExit();
                }
            }
        }
    }
}
